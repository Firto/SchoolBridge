using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolBridge.API.Controllers.Attributes;
using SchoolBridge.API.Controllers.Attributes.Validation;
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
        public async Task<ResultDto> Start([FromQuery, MyValidation] StartRegisterDto entity)
        {
            return ResultDto.Create(_registrationService.StartRegister(entity.Email, new Role { Name = "Pupil" }));
        }

        [UUID]
        [HttpPost]
        [MyNoAutorize]
        public async Task<ResultDto> End([FromBody, MyValidation]EndRegisterDto entity, [BindNever]string uuid)
        {
            return ResultDto.Create(await _registrationService.EndRegister(entity, uuid));
        }

        [HttpGet]
        [MyNoAutorize]
        public async Task<ResultDto> ChangePassword([FromQuery, ArgValid("str-input", "str-email")] string email)
        {
            return ResultDto.Create(_registrationService.ChangePasswordEmail(email));
        }

        [UUID]
        [HttpPost]
        [MyNoAutorize]
        public async Task<ResultDto> EndChangePassword([FromBody, MyValidation] EndChangePasswordEmailDto entity, [BindNever] string uuid)
        {
            return ResultDto.Create(await _registrationService.EndChangePasswordEmail(entity, uuid));
        }
    }
}