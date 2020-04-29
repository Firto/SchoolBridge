using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolBridge.API.Controllers.Attributes;
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
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IWebHostEnvironment _webHostEnvironment;
        IDataBaseNotificationService<User> _dataBaseNotificationManager;
        IGenericRepository<User> _userGR;
        IEmailService _emailService;
       
        public ValuesController(IWebHostEnvironment webHostEnvironment,
                                IDataBaseNotificationService<User> dataBaseNotificationManager,
                                IGenericRepository<User> userGR,
                                IEmailService emailService) {
            _webHostEnvironment = webHostEnvironment;
            _dataBaseNotificationManager = dataBaseNotificationManager;
            _userGR = userGR;
            _emailService = emailService;
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
        public async Task<ActionResult<IEnumerable<string>>> Test()
        {
            _emailService.SendByDraft("profliste@gmail.com", "high@schoolbridge.com", "TEST High", "email-registration", null, EmailEntityPriority.High, null, "https://github.com/Firto/SchoolBridge/");
            _emailService.SendByDraft("profliste@gmail.com", "high@schoolbridge.com", "TEST High", "email-registration", null, EmailEntityPriority.High, null, "https://github.com/Firto/SchoolBridge/");
            _emailService.SendByDraft("profliste@gmail.com", "high@schoolbridge.com", "TEST High", "email-registration", null, EmailEntityPriority.High, null, "https://github.com/Firto/SchoolBridge/");

            _emailService.SendByDraft("profliste@gmail.com", "medium@schoolbridge.com", "TEST Medium", "email-registration", null, EmailEntityPriority.Medium, null, "https://github.com/Firto/SchoolBridge/");
            _emailService.SendByDraft("profliste@gmail.com", "medium@schoolbridge.com", "TEST Medium", "email-registration", null, EmailEntityPriority.Medium, null, "https://github.com/Firto/SchoolBridge/");
            _emailService.SendByDraft("profliste@gmail.com", "medium@schoolbridge.com", "TEST Medium", "email-registration", null, EmailEntityPriority.Medium, null, "https://github.com/Firto/SchoolBridge/");

            _emailService.SendByDraft("profliste@gmail.com", "low@schoolbridge.com", "TEST Low", "email-registration", null, EmailEntityPriority.Low, null, "https://github.com/Firto/SchoolBridge/");
            _emailService.SendByDraft("profliste@gmail.com", "low@schoolbridge.com", "TEST Low", "email-registration", null, EmailEntityPriority.Low, null, "https://github.com/Firto/SchoolBridge/");
            _emailService.SendByDraft("profliste@gmail.com", "low@schoolbridge.com", "TEST Low", "email-registration", null, EmailEntityPriority.Low, null, "https://github.com/Firto/SchoolBridge/");
            return new string[] { _webHostEnvironment.ContentRootPath, _webHostEnvironment.WebRootPath };
        }
    }
}