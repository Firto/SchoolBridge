using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleGR;
        public RoleService(IGenericRepository<Role> roleGR)
        {
            _roleGR = roleGR;
        }

        public static void OnInit(ClientErrorManager manager)
        {
            manager.AddErrors(new ClientErrors("RoleService", new Dictionary<string, ClientError>
                {
                    { "inc-role-name", new ClientError("Incorrect role name!") },
                    { "inc-role-id", new ClientError("Incorrect role id!") }
                }));
        }

        public static void OnFirstInit(IGenericRepository<Role> roleGR)
        {
            roleGR.Create(new Role[] {
                    new Role { Name = "Admin" },
                    new Role { Name = "Pupil" }
            });
        }

        public Role Get(string name)
        {
            var ou = _roleGR.GetAll((x) => x.Name == name).FirstOrDefault();
            if (ou == null)
                throw new ClientException("inc-role-name");
            return ou;
        }


        public Role Get(int Id)
        {
            var ou = _roleGR.Find(Id);
            if (ou == null)
                throw new ClientException("inc-role-id");
            return ou;
        }

        public IEnumerable<Role> GetAll()
        {
            return _roleGR.GetAll();
        }

        public async Task<Role> GetAsync(string name)
        {
            var ou = (await _roleGR.GetAllAsync((x) => x.Name == name)).FirstOrDefault();
            if (ou == null)
                throw new ClientException("inc-role-name");
            return ou;
        }


        public async Task<Role> GetAsync(int Id)
        {
            var ou = await _roleGR.FindAsync(Id);
            if (ou == null)
                throw new ClientException("inc-role-id");
            return ou;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _roleGR.GetAllAsync();
        }
    }
}
