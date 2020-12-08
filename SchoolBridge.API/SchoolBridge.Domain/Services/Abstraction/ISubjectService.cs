using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Helpers.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface ISubjectService: IOnInitService
    {
        IEnumerable<PupilSubjectDto> GetAll(User user);

        PupilSubject Get(int id);
        PupilSubject GetByName(string name);

        void ChangeName(PupilSubject pupilSubject, string name);
        void ChangeComment(PupilSubject pupilSubject, string comment);
        void ChangePosition(PupilSubject pupilSubject, byte dayNumber, byte lessonNumber);

        //Async

        Task<IEnumerable<PupilSubjectDto>> GetAllAsync(User user);

        Task<PupilSubject> GetAsync(int id);
        Task<PupilSubject> GetByNameAsync(string name);

        Task<PupilSubject> AddUpdateSubjectAsync(SubjectDto entity, User user);
        Task RemoveSubjectAsync(PupilSubject pupilSubject);

        Task ChangeNameAsync(PupilSubject pupilSubject, string name);
        Task ChangeCommentAsync(PupilSubject pupilSubject, string comment);
        Task ChangePositionAsync(PupilSubject pupilSubject, byte dayNumber, byte lessonNumber);
    }
}