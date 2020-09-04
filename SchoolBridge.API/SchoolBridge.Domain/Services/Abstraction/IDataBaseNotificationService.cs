using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IDataBaseNotificationService: IOnInitService
    {
        Task NotifyAsync(string userId, string type, IDataBaseNotificationSourse sourse);
        Task NotifyAsync(string userId, string tokenId, string type, IDataBaseNotificationSourse sourse);
        Task NotifyAsync(string[] userIds, string type, IDataBaseNotificationSourse sourse);
        Task NotifyNoBaseAsync(string userId, string type, IDataBaseNotificationSourse sourse);
        Task NotifyNoBaseAsync(string userId, string tokenId, string type, IDataBaseNotificationSourse sourse);
        Task NotifyNoBaseAsync(string[] userIds, string type, IDataBaseNotificationSourse sourse);
        Task ReadAsync(string userId, string last);
        Task<IEnumerable<DataBaseSourse>> GetAsync(string userId, string last, int count = 20);
        Task<IEnumerable<DataBaseSourse>> GetAndReadAsync(string userId, string last, int count = 20);
        Task<int> GetCountUnreadAsync(string userId);
    }
}