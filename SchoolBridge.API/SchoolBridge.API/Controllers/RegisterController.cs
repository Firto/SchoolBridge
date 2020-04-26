using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolBridge.API.Controllers.Attributes;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Authefication;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegisterController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet]
        [MyNoAutorize]
        public async Task<ResultDto> Start(string email)
        {
            return ResultDto.Create(_registrationService.StartRegister(email, new Role { Id = 9 }));
        }

        [UUID]
        [HttpPost]
        [MyNoAutorize]
        public async Task<ResultDto> End([FromBody]EndRegisterDto entity, [BindNever]string uuid)
        {
            return ResultDto.Create(await _registrationService.EndRegister(entity, uuid));
        }
    }
}