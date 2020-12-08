using SchoolBridge.DataAccess.Entities.Chat;
using SchoolBridge.Helpers.AddtionalClases.DirectMessageService;
using SchoolBridge.Helpers.DtoModels.Chat;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IDirectMessagesService: IOnInitService
    {
        IEnumerable<DirectChatDto> GetChats(string tokenId, string userId);
        Task<IEnumerable<DirectChatDto>> GetChatsAsync(string tokenId, string userId);

        void Read(string chatId, string userId);
        Task ReadAsync(string chatId, string userId);

        IEnumerable<MessageDto> GetDirectMessages(string userId, string chatId, string last, int count = 20);
        Task<IEnumerable<MessageDto>> GetDirectMessagesAsync(string userId, string chatId, string last, int count = 20);

        MessageDto SendMessage(JwtSecurityToken token, string chatId, string type, IMessageSource message);
        Task<MessageDto> SendMessageAsync(JwtSecurityToken token, string chatId, string type, IMessageSource message);

        Task<MessageDto> SendFirstMessageAsync(JwtSecurityToken token, string userId, string type, IMessageSource message);
        MessageDto SendFirstMessage(JwtSecurityToken token, string userId, string type, IMessageSource message);
    }
}
