﻿using System.Threading.Tasks;
using SchoolBridge.API.Controllers.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.DtoModels;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class NotificationController : ControllerBase
    {
        private readonly IDataBaseNotificationService _dataBaseNotificationService;
        public NotificationController(IDataBaseNotificationService dataBaseNotificationService)
            => _dataBaseNotificationService = dataBaseNotificationService;

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> Get([BindNever]User user, [FromQuery]string last)
            => ResultDto.Create(await _dataBaseNotificationService.GetAsync(user.Id, last));

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> GetAndRead([BindNever]User user, [FromQuery]string last)
           => ResultDto.Create(await _dataBaseNotificationService.GetAndReadAsync(user.Id, last));

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> Read([BindNever]User user, [FromQuery]string last)
        {
            await _dataBaseNotificationService.ReadAsync(user.Id, last);
            return ResultDto.Create(null);
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> GetCountUnread([BindNever]User user)
            => ResultDto.Create(await _dataBaseNotificationService.GetCountUnreadAsync(user.Id));
    }
}