using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Helpers.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IProfileService: IOnInitService
    {
        Task<ProfileDto> ChangeProfile(User user, ProfileDto entity);
        Task ChangePassword(User user, ChangePasswordDto entity);
    }
}
