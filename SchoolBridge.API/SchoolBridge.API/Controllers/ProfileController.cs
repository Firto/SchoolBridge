﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolBridge.API.Controllers.Attributes;
using SchoolBridge.API.Controllers.Attributes.Validation;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.DtoModels;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        public ProfileController(IUserService userService)
            => _userService = userService;
        // security

        [HttpPost]
        [MyAutorize]
        public async Task<ResultDto> ChangeLogin([FromBody, MyValidation] ChangeLoginDto model, [BindNever]User user)
        {
            await _userService.ChangeLoginAsync(model.Login, user);
            return ResultDto.Create(null);
        }

        [HttpPost]
        [MyAutorize]
        public async Task<ResultDto> ChangeImage([FromBody, MyValidation] ChangeImageDto model, [BindNever]User user)
        {
            return ResultDto.Create(await _userService.ChangeImageAsync(model.Image, user));
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> Info([BindNever]User user)
        {
            ProfileDto model = new ProfileDto();
            user = _userService.Get(user.Id);
            model.Login = user.Login;
            model.Email = user.Email;
            model.Name = user.Name;
            model.Surname = user.Surname;
            model.Lastname = user.Lastname;
            model.Photo = user.PhotoId;
            return ResultDto.Create(model);
        }
    }
}
