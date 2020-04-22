using System.Threading.Tasks;
using SchoolBridge.API.Controllers.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.DtoModels.Authefication;
using SchoolBridge.DataAccess.Entities;

namespace GreenP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
            => _accountService = accountService;

        // security

        [UUID]
        [HttpPost]
        [MyNoAutorize]
        public async Task<ResultDto> Login([FromBody] LoginDto model, [BindNever]string uuid)
            => ResultDto.Create(await _accountService.Login(model, uuid));

        [UUID]
        [HttpPost]
        [MyNoAutorize]
        public async Task<ResultDto> RefreshToken([FromBody] RefreshTokenDto model, [BindNever]string uuid)
            => ResultDto.Create(await _accountService.RefreshToken(model, uuid));

        /*[UUID]
        [HttpPost]
        [MyNoAutorize]
        public async Task<ResultDto> Register([FromBody] RegisterDto model, [BindNever]string uuid)
            => ResultDto.Create(await _accountService.Register(model, uuid));*/

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> Logout()
        {
            await _accountService.Logout(HttpContext.Request.Headers);
            return ResultDto.Create(null); 
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> LogoutAll()
        {
            await _accountService.LogoutAll(HttpContext.Request.Headers);
            return ResultDto.Create(null);
        }

        // basic

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> GetProfile([BindNever]User user)
            => ResultDto.Create(await _accountService.GetProfileInfo(user));

        [HttpPost]
        [MyAutorize]
        [RequestSizeLimit(100 * 1024 * 1024)]     // set the maximum file size limit to 100 MB
        public async Task<ResultDto> SetProfile([FromBody]ProfileDto model, [BindNever]User user)
           => ResultDto.Create(await _accountService.SetProfileInfo(user, model));
    }
}