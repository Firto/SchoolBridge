﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolBridge.API.Controllers.Attributes;
using SchoolBridge.API.Controllers.Attributes.Globalization;
using SchoolBridge.API.Controllers.Attributes.Validation;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Globalization;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GlobalizationController : ControllerBase
    {
        private readonly ILanguageStringService _languageStringService;
        public GlobalizationController(ILanguageStringService languageStringService) 
        {
            _languageStringService = languageStringService;
        }

        [HttpPost, ActionName("language/add"), HasPermission("CreateLanguage")]
        public async Task<ResultDto> AddLanguage([FromBody, MyValidation]AddLanguageDto entity)
        {
            _languageStringService.AddLanguage(entity.AbbName, entity.FullName);
            return ResultDto.Create(entity);
        }

        [HttpGet, ActionName("language/remove"), HasPermission("RemoveLanguage")]
        public async Task<ResultDto> RemoveLanguage([ArgValid("str-input", "lss-lng-name")]string abbName)
        {
            _languageStringService.RemoveLanguage(abbName);
            return ResultDto.Create(null);
        }

        [HttpPost, ActionName("language/edit"), HasPermission("EditLanguage")]
        public async Task<ResultDto> EditLanguage([FromBody, MyValidation]GetLanguageEditDto entity)
        {
            _languageStringService.EditLanguage(entity.AbbName, entity.FullName);
            return ResultDto.Create(null);
        }

        [HttpPost, BaseUpatedId]
        public async Task<ResultDto> Strings([FromBody, MyValidation]GetLanguageStringsDto entity)
        {
            return ResultDto.Create(_languageStringService.GetByStringNameCurrent(entity.Strings));
        }

        [HttpPost, BaseUpatedId]
        public async Task<ResultDto> StringsByType([FromBody, MyValidation]GetLanguageStringsDto entity)
        {
            return ResultDto.Create(_languageStringService.GetByTypeCurrent(entity.Strings));
        }

        [HttpGet]
        public async Task<ResultDto> Info()
        {
            return ResultDto.Create(_languageStringService.GetGlobalizationInfo());
        }

        [HttpPost, ActionName("string/addorupdate"), HasPermission("EditGbStrings")]
        public async Task<ResultDto> StringAddOrUpdate([FromBody, MyValidation]AddOrUpdateStringDto entity)
        {
            _languageStringService.AddOrUpdateString(entity.Name, _languageStringService.GetCurrentLanguage(), entity.String);
            return ResultDto.Create(null);
        }

        [HttpGet, ActionName("update-baseupdateid"), HasPermission("UpdateBaseUpdateId")]
        public async Task<ResultDto> UpdateBaseUpdateId() 
        {
            _languageStringService.UpdateBaseUpdateId();
            return ResultDto.Create(_languageStringService.GetGlobalizationInfo());
        }
    }
}