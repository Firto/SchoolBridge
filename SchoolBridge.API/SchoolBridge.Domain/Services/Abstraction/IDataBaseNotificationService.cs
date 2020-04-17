using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IDataBaseNotificationService<AUser> where AUser : AuthUser
    {
        Task Notify(AUser usr, string type, IDataBaseNotificationSourse sourse);
        Task Notify(AUser usr, string tokenId, string type, IDataBaseNotificationSourse sourse);
        Task Notify(AUser[] usrs, string type, IDataBaseNotificationSourse sourse);
        Task Read(AUser usr, string last);
        Task<IEnumerable<DataBaseSourse>> Get(AUser usr, string last, int count = 20);
        Task<IEnumerable<DataBaseSourse>> GetAndRead(AUser usr, string last, int count = 20);
        Task<int> GetCountUnread(AUser usr);
    }
}