using SchoolBridge.DataAccess.Entities.Chat;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.DtoModels.Chat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using SchoolBridge.Helpers.AddtionalClases.DirectMessageService;
using System.Text;
using System;
using SchoolBridge.Helpers.AddtionalClases.ChatEventService.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService.ChatSources;
using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using SchoolBridge.Helpers.Extentions;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class DirectMessageService: IDirectMessagesService
    {
        private readonly IGenericRepository<DirectMessage> _directMessageGR;
        private readonly IGenericRepository<DirectChat> _directChatGR;
        private readonly IDataBaseNotificationService _dataBaseNotificationService;
        private readonly IUserService _userService;
        private readonly IJsonConverterService _jsonConverterService;
        private readonly IChatEventService _chatEventService;
        private readonly IMapper _mapper;

        public DirectMessageService(IGenericRepository<DirectMessage> directMessageGR,
                                    IGenericRepository<DirectChat> directChatGR,
                                    IDataBaseNotificationService dataBaseNotificationService,
                                    IUserService userService,
                                    IJsonConverterService jsonConverterService,
                                    IChatEventService chatEventService,
                                    IMapper mapper)
        {
            _directMessageGR = directMessageGR;
            _directChatGR = directChatGR;
            _dataBaseNotificationService = dataBaseNotificationService;
            _userService = userService;
            _jsonConverterService = jsonConverterService;
            _chatEventService = chatEventService;
            _mapper = mapper;
        }

        public static void OnInit(ClientErrorManager manager, IValidatingService validatingService) {
            manager.AddErrors(new ClientErrors("DirectMessagesService", new Dictionary<string, ClientError>() {
                { "inc-chatid", new ClientError("Incorrect chat!")},
                { "inc-message-id", new ClientError("Incorrect message id!")}
            }));

            validatingService.AddValidateFunc("dr-text-message", (string prop, PropValidateContext context) =>
            {
                if (prop == null) return;

                if (prop.Length < 1 || prop.Length > 300) 
                    context.Valid.Add($"[dr-text-message-inc-length, [pn-{context.PropName}]]");
            });
        }

        private string ObjectToBase64String(object obj)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(_jsonConverterService.ConvertObjectToJson(obj)));

        public IEnumerable<DirectChatDto> GetChats(string tokenId, string userId)
        {
            return _directChatGR.GetDbSet().Where(x => x.User1Id == userId || x.User2Id == userId).Include(x => x.Messages).OrderBy(x => x.LastModify).ToArray().Select(x => new DirectChatDto { 
                Id = x.Id,
                Read = !((userId == x.User1Id && x.Read == 1) || (userId == x.User2Id && x.Read == 2)),
                SubscribeToken = _chatEventService.CreateSubscriptionToken(tokenId, x.Id),
                LastMessage = _mapper.Map<DirectMessage, MessageDto>(x.Messages.OrderBy(x => x.Date).Last()),
                User = _userService.GetShortDto(x.User1Id == userId ? x.User2Id : x.User1Id)
            }).ToArray().Select(x => {
                if (x.LastMessage != null)
                    x.LastMessage.Sender = _userService.GetShortDto(x.LastMessage.Sender.Id);
                return x;
            });                
        }

        public async Task<IEnumerable<DirectChatDto>> GetChatsAsync(string tokenId, string userId)
        {
            return _directChatGR.GetDbSet().Where(x => x.User1Id == userId || x.User2Id == userId).Include(x => x.Messages).OrderBy(x => x.LastModify).ToArray().Select(x => new DirectChatDto
            {
                Id = x.Id,
                Read = !((userId == x.User1Id && x.Read == 1) || (userId == x.User2Id && x.Read == 2)),
                SubscribeToken = _chatEventService.CreateSubscriptionToken(tokenId, x.Id),
                LastMessage = x.Messages.Count() > 0 ? _mapper.Map<DirectMessage, MessageDto>(x.Messages.OrderBy(x => x.Date).Last()) : null,
                User = _userService.GetShortDto(x.User1Id == userId ? x.User2Id : x.User1Id)
            }).Select(x => {
                if (x.LastMessage != null) 
                    x.LastMessage.Sender = _userService.GetShortDto(x.LastMessage.Sender.Id);
                return x;
            });
        }

        public void Read(string chatId, string userId)
        {
            var chat = _directChatGR.GetDbSet().Where(x => x.Id == chatId && (x.User1Id == userId || x.User2Id == userId)).FirstOrDefault();
            if (chat == null)
                throw new ClientException("inc-chatid");

            chat.Read = 0;
            _directChatGR.Update(chat);
        }

        public async Task ReadAsync(string chatId, string userId)
        {
            var chat = await _directChatGR.GetDbSet().Where(x => x.Id == chatId && (x.User1Id == userId || x.User2Id == userId)).FirstOrDefaultAsync();
            if (chat == null)
                throw new ClientException("inc-chatid");

            chat.Read = 0;
            await _directChatGR.UpdateAsync(chat);
        }

        public IEnumerable<DirectChatDto> GetDirectMessages(string userId, string chatId, string last, int count = 20)
        {
            var chat = _directChatGR.GetDbSet().Where(x => x.Id == chatId && (x.User1Id == userId || x.User2Id == userId)).FirstOrDefault();
            if (chat == null)
                throw new ClientException("inc-chatid");

            DirectMessage message = null;
            if (last != null)
            {
                message =  _directMessageGR.GetDbSet().Where(x => x.Id == last && x.ChatId == chatId).FirstOrDefault();
                if (message == null)
                    throw new ClientException("inc-message-id", last);
            }

            return _mapper.Map<IEnumerable<DirectMessage>, IEnumerable<DirectChatDto>>(
                _directMessageGR.GetDbSet()
                .Where((x) => x.ChatId == chatId && (message == null || x.Date < message.Date))
                .OrderBy((x) => x.Date)
                .Take(count)
                .ToArray()
           );
        }

        public async Task<IEnumerable<DirectChatDto>> GetDirectMessagesAsync(string userId, string chatId, string last, int count = 20)
        {
            var chat = _directChatGR.GetDbSet().Where(x => x.Id == chatId && (x.User1Id == userId || x.User2Id == userId)).FirstOrDefault();
            if (chat == null)
                throw new ClientException("inc-chatid");

            DirectMessage message = null;
            if (last != null)
            {
                message = await _directMessageGR.GetDbSet().Where(x => x.Id == last && x.ChatId == chatId).FirstOrDefaultAsync();
                if (message == null)
                    throw new ClientException("inc-message-id", last);
            }
            
            return _mapper.Map<IEnumerable<DirectMessage>, IEnumerable<DirectChatDto>>(
                await _directMessageGR.GetDbSet()
                .Where((x) => x.ChatId == chatId && (message == null || x.Date < message.Date))
                .OrderBy((x) => x.Date)
                .Take(count)
                .ToArrayAsync()
           );
        }

        public MessageDto SendMessage(JwtSecurityToken token, string chatId, string type, IMessageSource message)
        {
            var chat = _directChatGR.GetDbSet().Where(x => x.Id == chatId && (x.User1Id == token.Subject || x.User2Id == token.Subject)).FirstOrDefault();
            if (chat == null)
                throw new ClientException("inc-chatid");

            var msg = _directMessageGR.Create(new DirectMessage
            {
                ChatId = chatId,
                SenderId = token.Subject,
                Type = type,
                Date = DateTime.Now,
                Base64Source = ObjectToBase64String(message)
            });

            chat.Read = chat.User1Id == token.Subject ? 2 : 1;
            _directChatGR.Update(chat);

            var ntf = new ChatMessageSource
            {
                Id = msg.Id,
                Type = msg.Type,
                Base64Source = msg.Base64Source,
                Date = msg.Date,
                Sender = _userService.GetShortDto(token.Subject)
            };

            _dataBaseNotificationService.NotifyAsync(chat.User1Id == token.Subject ? chat.User2Id : chat.User1Id, "newChatMessage", ntf);

            var msgDto = new NewMessageSource
            {
                Id = msg.Id,
                Type = msg.Type,
                Base64Source = msg.Base64Source,
                Date = msg.Date,
                Sender = ntf.Sender
            };
            _chatEventService.SendEventAsync(chatId, "newMessage", msgDto);

            return msgDto;
        }

        public async Task<MessageDto> SendMessageAsync(JwtSecurityToken token, string chatId, string type, IMessageSource message)
        {
            var chat = await _directChatGR.GetDbSet().Where(x => x.Id == chatId && (x.User1Id == token.Subject || x.User2Id == token.Subject)).FirstOrDefaultAsync();
            if (chat == null )
                throw new ClientException("inc-chatid");

            var msg = await _directMessageGR.CreateAsync(new DirectMessage
            {
                ChatId = chatId,
                SenderId = token.Subject,
                Type = type,
                Date = DateTime.Now,
                Base64Source = ObjectToBase64String(message)
            });

            chat.Read = chat.User1Id == token.Subject ? 2 : 1;
            await _directChatGR.UpdateAsync(chat);

            var ntf = new ChatMessageSource
            {
                Date = msg.Date,
                Sender = _userService.GetStaticShortDto(token.Subject),
                Type = msg.Type,
                Base64Source = msg.Base64Source
            };

            await _dataBaseNotificationService.NotifyAsync(chat.User1Id == token.Subject ? chat.User2Id : chat.User1Id, "newChatMessage", ntf);

            var msgDto = new NewMessageSource
            {
                Id = msg.Id,
                Type = msg.Type,
                Base64Source = msg.Base64Source,
                Date = msg.Date,
                Sender = ntf.Sender
            };

            await _chatEventService.SendEventAsync(chatId, "newMessage", msgDto);

            return msgDto;
        }

        public MessageDto SendFirstMessage(JwtSecurityToken token, string userId, string type, IMessageSource message)
        {
            var chat = _directChatGR.GetDbSet().Where(x => (x.User1Id == token.Subject || x.User2Id == token.Subject) && (x.User1Id == userId || x.User2Id == userId)).FirstOrDefault();
            if (chat != null || token.Subject == userId)
                throw new ClientException("inc-chatid");
            _userService.Get(userId);
            chat = _directChatGR.Create(new DirectChat
            {
                LastModify = DateTime.Now,
                Messages = new List<DirectMessage>() {
                    _directMessageGR.Create(new DirectMessage
                        {

                            SenderId = token.Subject,
                            Type = type,
                            Date = DateTime.Now,
                            Base64Source = ObjectToBase64String(message)
                        })
                    },
                Read = chat.User1Id == token.Subject ? 2 : 1,
                User1Id = token.Subject,
                User2Id = userId
            });

            var msg = chat.Messages.Last();

            var ntf = new ChatMessageSource
            {
                Date = msg.Date,
                Sender = _userService.GetStaticShortDto(token.Subject),
                Type = msg.Type,
                Base64Source = msg.Base64Source
            };

            _dataBaseNotificationService.NotifyAsync(chat.User1Id == token.Subject ? chat.User2Id : chat.User1Id, "newChatMessage", ntf);

            var msgDto = new NewMessageSource
            {
                Id = msg.Id,
                Type = msg.Type,
                Base64Source = msg.Base64Source,
                Date = msg.Date,
                Sender = ntf.Sender
            };

            _chatEventService.SendEventAsync(chat.Id, "newMessage", msgDto);

            return msgDto;
        }

        public async Task<MessageDto> SendFirstMessageAsync(JwtSecurityToken token, string userId, string type, IMessageSource message)
        {
            var chat = await _directChatGR.GetDbSet().Where(x => (x.User1Id == token.Subject || x.User2Id == token.Subject) && (x.User1Id == userId || x.User2Id == userId)).FirstOrDefaultAsync();
            if (chat != null || token.Subject == userId)
                throw new ClientException("inc-chatid");
            await _userService.GetAsync(userId);
            chat = await _directChatGR.CreateAsync(new DirectChat
            {
                LastModify = DateTime.Now,
                Messages = new List<DirectMessage>() {
                    _directMessageGR.Create(new DirectMessage
                        {

                            SenderId = token.Subject,
                            Type = type,
                            Date = DateTime.Now,
                            Base64Source = ObjectToBase64String(message)
                        })
                    },
                Read = chat.User1Id == token.Subject ? 2 : 1,
                User1Id = token.Subject,
                User2Id = userId
            });

            var msg = chat.Messages.Last();

            var ntf = new ChatMessageSource
            {
                Date = msg.Date,
                Sender = _userService.GetStaticShortDto(token.Subject),
                Type = msg.Type,
                Base64Source = msg.Base64Source
            };

            await _dataBaseNotificationService.NotifyAsync(chat.User1Id == token.Subject ? chat.User2Id : chat.User1Id, "newChatMessage", ntf);

            var msgDto = new NewMessageSource
            {
                Id = msg.Id,
                Type = msg.Type,
                Base64Source = msg.Base64Source,
                Date = msg.Date,
                Sender = ntf.Sender
            };

            await _chatEventService.SendEventAsync(chat.Id, "newMessage", msgDto);

            return msgDto;
        }
    }
}
