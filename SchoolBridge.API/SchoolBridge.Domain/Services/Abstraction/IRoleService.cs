using SchoolBridge.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IRoleService
    {
        Task<Role> Get(string name);
        Task<Role> Get(int Id);
        Task<IEnumerable<Role>> GetAll();
    }
}
