using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolBridge.API.Controllers.Attributes;
using SchoolBridge.API.Controllers.Attributes.Validation;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Implementation;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.AddtionalClases.DirectMessageService.MessageSources;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Authefication;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ChatController : ControllerBase
    {
        private readonly IDirectMessagesService _directMessagesService;

        public ChatController(IDirectMessagesService directMessagesService)
        {
            _directMessagesService = directMessagesService;
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> GetChats([BindNever] JwtSecurityToken token)
        {
            return ResultDto.Create(await _directMessagesService.GetChatsAsync(token.Id, token.Subject));
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> GetMessages([BindNever] JwtSecurityToken token, [FromQuery, ArgValid("str-input")]string chatId, [FromQuery]string last)
        {
            return ResultDto.Create(await _directMessagesService.GetDirectMessagesAsync(token.Subject, chatId, last));
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> SendMessage([BindNever] JwtSecurityToken token, [FromQuery, ArgValid("str-input")]string chatId, [FromQuery, ArgValid("str-input", "dr-text-message")]string text)
        {
            return ResultDto.Create(await _directMessagesService.SendMessageAsync(token, chatId, "text", new TextMessage { Text = text }));
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> SendFirstMessage([BindNever] JwtSecurityToken token, [FromQuery, ArgValid("str-input")]string userId, [FromQuery, ArgValid("str-input", "dr-text-message")]string text)
        {
            return ResultDto.Create(await _directMessagesService.SendFirstMessageAsync(token, userId, "text", new TextMessage { Text = text }));
        }
    }
}