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
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Authefication;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _usersGR;
        private readonly IOnlineService _onlineService;
        private readonly IUserService _userService;

        public UserController(IGenericRepository<User> usersGR,
                               IOnlineService onlineService,
                               IUserService userService)
        {
            _usersGR = usersGR;
            _onlineService = onlineService;
            _userService = userService;
        }

        [HttpGet]
        [HasPermission("GetAllUsers")]
        public async Task<ResultDto> GetAll([BindNever] JwtSecurityToken token)
        {
            return ResultDto.Create((await _usersGR.GetAllIncludeAsync(x => true, x => x.Role)).Select(x => new UserDto { Id = x.Id, Login = x.Login, Role = x.Role.Name, OnlineStatus = _onlineService.GetOnlineStatus(x.Id), OnlineStatusSubscriptionToken = _onlineService.CreateOnlineStatusSubscriptionToken(token.Id, x.Id) }));
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> GetAllShort([BindNever] JwtSecurityToken token)
        {
            return ResultDto.Create((await _usersGR.GetAllAsync()).Select(x => _userService.GetShortDto(x.Id)));
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> Get([FromQuery, ArgValid("str-input")]string getToken, [BindNever] JwtSecurityToken token)
        {
            return ResultDto.Create(await _userService.GetFullDtoByGetTokenAsync(token.Id, getToken));
        }

        [HttpPost]
        [MyAutorize]
        public async Task<ResultDto> GetMany([FromBody, MyValidation] GetManyUsersDto dto, [BindNever] JwtSecurityToken token)
        {
            return ResultDto.Create(dto.GetTokens.Select(x => _userService.GetFullDtoByGetToken(token.Id, x)));
        }
    }
}