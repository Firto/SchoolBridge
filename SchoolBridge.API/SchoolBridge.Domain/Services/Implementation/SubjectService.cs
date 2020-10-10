using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class SubjectService : ISubjectService
    {
        private readonly IGenericRepository<PupilSubject> _subjectGR;

        public SubjectService(IGenericRepository<PupilSubject> subjectGR)
        {
            _subjectGR = subjectGR;
        }

        public static void OnInit(ClientErrorManager manager, IValidatingService validatingService, SubjectServiceConfiguration configuration)
        {
            manager.AddErrors(new ClientErrors("PupilSubject", new Dictionary<string, ClientError>
                {
                    { "u-inc-subject-id" , new ClientError ("Incorrect subject id!")}
                }));

            validatingService.AddValidateFunc("str-sb-name", (string prop, PropValidateContext ctx) => {
                if (prop == null) return;

                if (prop.Length > configuration.MaxCountCharsName)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {255}]");
                if (prop.Length < configuration.MinCountCharsName)
                    ctx.Valid.Add($"[str-too-short, [pn-{ctx.PropName}], {255}]");
                if (!Regex.Match(prop, "^[ а-яА-ЯҐґЄєІіЇї]+$").Success)
                    ctx.Valid.Add($"[str-no-spc-ch-2, [pn-{ctx.PropName}]]"); //"Name musn't have specials chars!"
            });
            validatingService.AddValidateFunc("str-sb-comment", (string prop, PropValidateContext ctx) => {
                if (prop == null) return;

                if (prop.Length > configuration.MaxCountCharsComment)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {255}]");
                if (!Regex.Match(prop, "^[ а-яА-ЯҐґЄєІіЇї]+$").Success)
                    ctx.Valid.Add($"[str-no-spc-ch-2, [pn-{ctx.PropName}]]"); //"Comment musn't have specials chars!"
            });
            validatingService.AddValidateFunc("number-day-sb", (string prop, PropValidateContext ctx) => {
                int value;
                if (prop == null || !Int32.TryParse(prop, out value)) return;

                if (value > configuration.MaxDayNumber || value < configuration.MinDayNumber)
                    ctx.Valid.Add($"[inc-number-day]");
            });
            validatingService.AddValidateFunc("number-lesson-sb", (string prop, PropValidateContext ctx) => {
                int value;
                if (prop == null || !Int32.TryParse(prop, out value)) return;

                if (value > configuration.MaxLessonNumber || value < configuration.MinLessonNumber)
                    ctx.Valid.Add($"[inc-number-lesson]");
            });
        }

        public PupilSubject Get(int id)
        {
            return _subjectGR.Find(id);
        }

        public async Task<PupilSubject> GetAsync(int id)
        {
            return await _subjectGR.FindAsync(id);
        }

        public PupilSubject GetByName(string name)
        {
            return _subjectGR.GetAll(x => x.SubjectName == name).FirstOrDefault();
        }

        public async Task<PupilSubject> GetByNameAsync(string name)
        {
            return (await _subjectGR.GetAllAsync(x => x.SubjectName == name)).FirstOrDefault();
        }

        public async Task<PupilSubject> AddUpdateSubjectAsync(SubjectDto entity, User user)
        {
            var pupilSubject = (await _subjectGR.GetAllAsync(x => x.LessonNumber == Int32.Parse(entity.LessonNumber) && x.DayNumber == Int32.Parse(entity.DayNumber) && x.PupilId == user.Id)).FirstOrDefault();
            bool isUpdate = true;
            if (pupilSubject == null)
            {
                pupilSubject = new PupilSubject
                {
                    PupilId = user.Id
                };
                isUpdate = false;
            }
            pupilSubject.SubjectName = entity.SubjectName;
            pupilSubject.Comment = entity.Comment;
            pupilSubject.DayNumber = Byte.Parse(entity.DayNumber);
            pupilSubject.LessonNumber = Byte.Parse(entity.LessonNumber);
            if (isUpdate)
                await _subjectGR.UpdateAsync(pupilSubject);
            else
                pupilSubject = await _subjectGR.CreateAsync(pupilSubject);
            return pupilSubject;
        }

        public void ChangeComment(PupilSubject pupilSubject, string comment)
        {
            pupilSubject = _subjectGR.Find(pupilSubject.Id);
            pupilSubject.Comment = comment;
            _subjectGR.Update(pupilSubject);
        }

        public async Task ChangeCommentAsync(PupilSubject pupilSubject, string comment)
        {
            pupilSubject = await _subjectGR.FindAsync(pupilSubject.Id);
            pupilSubject.Comment = comment;
            await _subjectGR.UpdateAsync(pupilSubject);
        }

        public void ChangeName(PupilSubject pupilSubject, string name)
        {
            pupilSubject = _subjectGR.Find(pupilSubject.Id);
            pupilSubject.SubjectName = name;
            _subjectGR.Update(pupilSubject);
        }

        public async Task ChangeNameAsync(PupilSubject pupilSubject, string name)
        {
            pupilSubject = await _subjectGR.FindAsync(pupilSubject.Id);
            pupilSubject.SubjectName = name;
            await _subjectGR.UpdateAsync(pupilSubject);
        }

        public void ChangePosition(PupilSubject pupilSubject, byte dayNumber, byte lessonNumber)
        {
            pupilSubject = _subjectGR.Find(pupilSubject.Id);
            pupilSubject.DayNumber = dayNumber;
            pupilSubject.LessonNumber = lessonNumber;
            _subjectGR.Update(pupilSubject);
        }

        public async Task ChangePositionAsync(PupilSubject pupilSubject, byte dayNumber, byte lessonNumber)
        {
            pupilSubject = await _subjectGR.FindAsync(pupilSubject.Id);
            pupilSubject.DayNumber = dayNumber;
            pupilSubject.LessonNumber = lessonNumber;
            await _subjectGR.UpdateAsync(pupilSubject);
        }

        public async Task RemoveSubjectAsync(PupilSubject pupilSubject)
        {
            await _subjectGR.DeleteAsync(pupilSubject);
        }

        public PupilSubject GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<PupilSubject> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
