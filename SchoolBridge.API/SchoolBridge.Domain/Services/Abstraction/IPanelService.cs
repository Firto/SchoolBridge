using SchoolBridge.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IPanelService
    {
        Task<IEnumerable<Panel>> GetAllPanels();
        Task<IEnumerable<Panel>> GetPanels(User user);
        Task<IEnumerable<Panel>> GetDefaultPanels(Role role);
        Task<IEnumerable<Panel>> GetDefaultPanels(string role);

        Task<UserPanel> AddPanel(User user, Panel panel);
        Task<IEnumerable<UserPanel>> AddPanels(User user, IEnumerable<Panel> panels);
        Task<RolePanel> AddPanelDefault(Role role, Panel panel);
        Task<RolePanel> AddPanelDefault(string role, Panel panel);
        Task<IEnumerable<RolePanel>> AddPanelsDefault(Role role, IEnumerable<Panel> panels);
        Task<IEnumerable<RolePanel>> AddPanelsDefault(string role, IEnumerable<Panel> panels);

        Task RemovePanel(User user, Panel panel);
        Task RemovePanels(User user, IEnumerable<Panel> panels);
        Task RemovePanelDefault(Role role, Panel panel);
        Task RemovePanelDefault(string role, Panel panel);
        Task RemovePanelsDefault(Role role, IEnumerable<Panel> panels);
        Task RemovePanelsDefault(string role, IEnumerable<Panel> panels);
    }
}
