using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class PanelService: IPanelService
    {
        private readonly IGenericRepository<Panel> _panelGR;
        private readonly IGenericRepository<UserPanel> _userPanelGR;
        private readonly IGenericRepository<RolePanel> _rolePanelGR;
        private readonly IRoleService _roleService;
        public PanelService(IGenericRepository<Panel> panelGR,
                            IGenericRepository<UserPanel> userPanelGR,
                            IGenericRepository<RolePanel> rolePanelGR,
                            IMapper mapper,
                            IRoleService roleService,
                            ClientErrorManager clientErrorManager)
        {
            _panelGR = panelGR;
            _userPanelGR = userPanelGR;
            _rolePanelGR = rolePanelGR;
            _roleService = roleService;

            if (!clientErrorManager.IsIssetErrors("Panel"))
                clientErrorManager.AddErrors(new ClientErrors("Panel", new Dictionary<string, ClientError> {
                    
                }));
        }

        public async Task<UserPanel> AddPanel(User user, Panel panel)
        {
            return await _userPanelGR.CreateAsync(new UserPanel { UserId = user.Id, PanelId = panel.Id });
        }

        public async Task<RolePanel> AddPanelDefault(Role role, Panel panel)
        {
            return await _rolePanelGR.CreateAsync(new RolePanel { RoleId = role.Id, PanelId = panel.Id });
        }

        public async Task<RolePanel> AddPanelDefault(string role, Panel panel)
        {
            return await _rolePanelGR.CreateAsync(new RolePanel { Role = await _roleService.Get(role), PanelId = panel.Id });
        }

        public async Task<IEnumerable<UserPanel>> AddPanels(User user, IEnumerable<Panel> panels)
        {
            return await _userPanelGR.CreateAsync(panels.Select(x => new UserPanel { UserId = user.Id, PanelId = x.Id}));
        }

        public async Task<IEnumerable<RolePanel>> AddPanelsDefault(Role role, IEnumerable<Panel> panels)
        {
            return await _rolePanelGR.CreateAsync(panels.Select(x => new RolePanel { RoleId = role.Id, PanelId = x.Id }));
        }

        public async Task<IEnumerable<RolePanel>> AddPanelsDefault(string role, IEnumerable<Panel> panels)
        {
            return await AddPanelsDefault(await _roleService.Get(role), panels);
        }

        public async Task<IEnumerable<Panel>> GetAllPanels()
        {
            return await _panelGR.GetAllAsync();
        }

        public async Task<IEnumerable<Panel>> GetDefaultPanels(Role role)
        {
            return (await _rolePanelGR.GetAllIncludeAsync((x) => x.RoleId == role.Id, (s) => s.Panel)).Select(x => new Panel { Id = x.PanelId});
        }

        public async Task<IEnumerable<Panel>> GetDefaultPanels(string role)
        {
            var ro = await _roleService.Get(role);
            return await GetDefaultPanels(ro);
        }

        public async Task<IEnumerable<Panel>> GetPanels(User user)
        {
            return (await _userPanelGR.GetAllIncludeAsync((x) => x.UserId == user.Id, (s) => s.Panel)).Select(x => new Panel { Id = x.PanelId });
        }

        public async Task RemovePanel(User user, Panel panel)
        {
            await _userPanelGR.DeleteAsync((x) => x.UserId == user.Id && x.PanelId == panel.Id);
        }

        public async Task RemovePanelDefault(Role role, Panel panel)
        {
            await _rolePanelGR.DeleteAsync((x) => x.RoleId == role.Id && x.PanelId == panel.Id);
        }

        public async Task RemovePanelDefault(string role, Panel panel)
        {
            var ro = await _roleService.Get(role);
            await _rolePanelGR.DeleteAsync((x) => x.RoleId == ro.Id && x.PanelId == panel.Id);
        }

        public async Task RemovePanels(User user, IEnumerable<Panel> panels)
        {
            await _userPanelGR.DeleteAsync((x) => x.UserId == user.Id && panels.FirstOrDefault((s) => s.Id == x.PanelId) != null);
        }

        public async Task RemovePanelsDefault(Role role, IEnumerable<Panel> panels)
        {
            await _rolePanelGR.DeleteAsync((x) => x.RoleId == role.Id && panels.FirstOrDefault((s) => s.Id == x.PanelId) != null);
        }

        public async Task RemovePanelsDefault(string role, IEnumerable<Panel> panels)
        {
            var ro = await _roleService.Get(role);
            await _rolePanelGR.DeleteAsync((x) => x.RoleId == ro.Id && panels.FirstOrDefault((s) => s.Id == x.PanelId) != null);
        }
    }
}
