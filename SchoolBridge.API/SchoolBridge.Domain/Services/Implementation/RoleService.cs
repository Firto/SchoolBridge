using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleGR;
        public RoleService( IGenericRepository<Role> roleGR,
                            ClientErrorManager clientErrorManager)
        {
            _roleGR = roleGR;

            if (!clientErrorManager.IsIssetErrors("Role"))
                clientErrorManager.AddErrors(new ClientErrors("Role", new Dictionary<string, ClientError>
                {
                    { "inc-role-name", new ClientError("Incorrect role name!") },
                    { "inc-role-id", new ClientError("Incorrect role id!") }
                }));
        }

        public async Task<Role> Get(string name)
        {
            var ou = (await _roleGR.GetAllAsync((x) => x.Name == name)).FirstOrDefault();
            if (ou == null)
                throw new ClientException("inc-role-name");
            return ou;
        }

        public async Task<Role> Get(int Id)
        {
            var ou = await _roleGR.FindAsync(Id);
            if (ou == null)
                throw new ClientException("inc-role-id");
            return ou;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _roleGR.GetAllAsync();
        }
    }
}
