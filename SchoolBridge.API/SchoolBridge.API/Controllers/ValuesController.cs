﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;

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
        public async Task<ActionResult<IEnumerable<string>>> SendEmail()
        {
            _emailService.SendDefaultByDraft("profliste@gmail.com", "Registration", "email-registration", null, null, "https://github.com/Firto/SchoolBridge/");
            return new string[] { _webHostEnvironment.ContentRootPath, _webHostEnvironment.WebRootPath };
        }
    }
}