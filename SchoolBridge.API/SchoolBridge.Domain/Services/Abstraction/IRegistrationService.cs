using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Authefication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IRegistrationService
    {
        string CreateRegistrationToken(TimeSpan? exp, string email, Role role, IEnumerable<Panel> noPanels = null, IEnumerable<Permission> noPermissions = null);
        string CreateRegistrationToken(TimeSpan? exp, string email, Role role);
        Task<string> CreateRegistrationToken(TimeSpan? exp, string email, string role);
        PermanentSubscribeDto StartRegister(string email, Role role, IEnumerable<Panel> noPanels, IEnumerable<Permission> noPermissions);
        PermanentSubscribeDto StartRegister(string email, Role role);
        Task<PermanentSubscribeDto> StartRegister(string email, string role);

        Task<User> EndRegister(EndRegisterDto entity);
    }
}
