using System.Collections.Generic;
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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [MyAutorize]
        public async Task<ResultDto> GetAll([BindNever] User user)
        {
            return ResultDto.Create(await _subjectService.GetAllAsync(user));
        }

        [HttpPost]
        [MyAutorize]
        public async Task<ResultDto> AddOrUpdate([FromBody, MyValidation] SubjectDto entity, [BindNever] User user)
        {
            var mm = await _subjectService.AddUpdateSubjectAsync(entity, user);
            var kk = new PupilSubjectDto();
            kk.Day = mm.DayNumber;
            kk.Lesson = mm.DayNumber;
            kk.LessonName = mm.SubjectName;
            kk.Description = mm.Comment;
            return ResultDto.Create(kk);
        }
    }
}