using SchoolBridge.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IRoleService: IOnInitService, IOnFirstInitService
    {
        Role Get(string name);
        Role Get(int Id);
        IEnumerable<Role> GetAll();

        Task<Role> GetAsync(string name);
        Task<Role> GetAsync(int Id);
        Task<IEnumerable<Role>> GetAllAsync();
    }
}
