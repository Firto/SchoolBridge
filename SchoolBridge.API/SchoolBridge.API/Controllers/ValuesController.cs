using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Authefication;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IWebHostEnvironment _webHostEnvironment;
        IDataBaseNotificationService<User> _dataBaseNotificationManager;
        IGenericRepository<User> _userGR;
        IEmailService _emailService;
        IRegistrationService _registrationService;
        public ValuesController(IWebHostEnvironment webHostEnvironment,
                                IDataBaseNotificationService<User> dataBaseNotificationManager,
                                IGenericRepository<User> userGR,
                                IEmailService emailService,
                                IRegistrationService registrationService) {
            _webHostEnvironment = webHostEnvironment;
            _dataBaseNotificationManager = dataBaseNotificationManager;
            _userGR = userGR;
            _emailService = emailService;
            _registrationService = registrationService;
        }
        // GET api/values
        [HttpGet]
        [ActionName("")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            await _dataBaseNotificationManager.Notify(_userGR.GetAll((x) => x.Login == "admin").First(), "test", new MessageNotificationSource { Message = "Baboola" });
            return new string[] { _webHostEnvironment.ContentRootPath, _webHostEnvironment.WebRootPath };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> SendEmail()
        {
            _emailService.SendByDraft("profliste@gmail.com", "test@schoolbridge.com", "TEST", "email-registration", null, null, "https://github.com/Firto/SchoolBridge/");
            return new string[] { _webHostEnvironment.ContentRootPath, _webHostEnvironment.WebRootPath };
        }

        public async Task<PermanentSubscribeDto> RegisterStart()
        {
            return _registrationService.StartRegister("profliste@gmail.com", new Role { Id = 1 }, new Panel[]{ new Panel { Id = 9} }, new Permission[] { new Permission { Id = 5 } });
        }
        [HttpPost]
        public async Task<User> RegisterEnd(EndRegisterDto entity)
        {
            return await _registrationService.EndRegister(entity);
        }
    }
}