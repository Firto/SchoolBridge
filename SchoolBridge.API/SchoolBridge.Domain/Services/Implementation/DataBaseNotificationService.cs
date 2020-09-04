using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SchoolBridge.Domain.SignalR.Hubs;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class DataBaseNotificationService : IDataBaseNotificationService
    {
       
        private readonly INotificationService _notificationService;
        private readonly IGenericRepository<Notification> _notificationGR;
        private readonly IJsonConverterService _jsonConverterService;
        private readonly IMapper _mapper;
        private readonly string methodReadName = "onReadNtf";
        public DataBaseNotificationService(INotificationService notificationService,
                                        IGenericRepository<Notification> notificationGR,
                                        IJsonConverterService jsonConverterService,
                                        IMapper mapper)
        {
            _notificationService = notificationService;
            _notificationGR = notificationGR;
            _jsonConverterService = jsonConverterService;
            _mapper = mapper;
        }

        public static void OnInit(ClientErrorManager manager) {
            manager.AddErrors(new ClientErrors("DataBaseNotificationService",
                new Dictionary<string, ClientError>
                {
                        {"inc-ntf-id", new ClientError("Incorrect notification id!")}
                }
           ));
        }

        private string ObjectToBase64String(object obj)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(_jsonConverterService.ConvertObjectToJson(obj)));

        public async Task NotifyAsync(string userId, string type, IDataBaseNotificationSourse sourse)
        {
            await _notificationService.NotifyAsync(userId, _mapper.Map<Notification, DataBaseSourse>(
                await _notificationGR.CreateAsync(
                    new Notification
                    {
                        Date = DateTime.Now,
                        Base64Sourse = ObjectToBase64String(sourse),
                        Type = type,
                        UserId = userId
                    }
                ))
            , "dataBase");
        }

        public async Task NotifyAsync(string userId, string tokenId, string type, IDataBaseNotificationSourse sourse)
        {
            await _notificationService.NotifyAsync(userId, tokenId, "dataBase", _mapper.Map<Notification, DataBaseSourse>(
                await _notificationGR.CreateAsync(
                    new Notification
                    {
                        Date = DateTime.Now,
                        Base64Sourse = ObjectToBase64String(sourse),
                        Type = type,
                        UserId = userId
                    }
                ))
            );
        }

        public async Task NotifyAsync(string[] userIds, string type, IDataBaseNotificationSourse sourse)
        {
            var template = new Notification
            {
                Date = DateTime.Now,
                Base64Sourse = ObjectToBase64String(sourse),
                Type = type
            };
            var outTemplate = _mapper.Map<Notification, DataBaseSourse>(template);
            Notification temp;
            var notifications = userIds.Select(x =>
            {
                temp = (Notification)template.Clone();
                temp.UserId = x;
                return temp;
            });
            (await _notificationGR.CreateAsync(notifications)).ForEach(async (x) =>
                    await _notificationService.NotifyAsync(x.UserId, outTemplate, "dataBase")
            );
        }

        public async Task NotifyNoBaseAsync(string userId, string type, IDataBaseNotificationSourse sourse)
        {
            await _notificationService.NotifyAsync(userId,
                new DataBaseSourse
                {
                    Date = DateTime.Now,
                    Base64Sourse = ObjectToBase64String(sourse),
                    Type = type
                }
            ,"dataBase");
        }

        public async Task NotifyNoBaseAsync(string userId, string tokenId, string type, IDataBaseNotificationSourse sourse)
        {
            await _notificationService.NotifyAsync(userId, tokenId, "dataBase", 
                new DataBaseSourse
                {
                    Date = DateTime.Now,
                    Base64Sourse = ObjectToBase64String(sourse),
                    Type = type
                }
            );
        }

        public async Task NotifyNoBaseAsync(string[] userIds, string type, IDataBaseNotificationSourse sourse)
        {
            var template = new DataBaseSourse
            {
                Date = DateTime.Now,
                Base64Sourse = ObjectToBase64String(sourse),
                Type = type
            };
            await _notificationService.NotifyAsync(userIds, template, "dataBase");
        }

        public async Task<IEnumerable<DataBaseSourse>> GetAsync(string userId, string last, int count = 20)
        {
            Notification notification = last != null ? await _notificationGR.FindAsync(last) : null;
            if (last != null && notification == null)
                throw new ClientException("inc-ntf-id", last);
            return _mapper.Map<IEnumerable<Notification>, IEnumerable<DataBaseSourse>>(_notificationGR.GetDbSet().Where((x) => x.UserId == userId && (notification == null || x.Date < notification.Date)).OrderByDescending((x) => x.Date).Take(count).ToArray());
        }

        public async Task<IEnumerable<DataBaseSourse>> GetAndReadAsync(string userId, string last, int count = 20)
        {
            Notification notification = last != null ? await _notificationGR.FindAsync(last) : null;
            if (last != null && notification == null)
                throw new ClientException("inc-ntf-id", last);
            var ntfs = _notificationGR.GetDbSet()
                                                   .Where((x) => x.UserId == userId && (notification == null || x.Date < notification.Date))
                                                   .OrderByDescending((x) => x.Date).Take(count)
                                                   .AsEnumerable();
            var ms = ntfs.Where((x) => x.Read == false).Select((x) => { x.Read = true; return x; });
            _notificationGR.GetDbContext().UpdateRange(ms);
            var mon = await _notificationGR.GetDbContext().SaveChangesAsync();
            if (mon > 0)
                await ReadAsync(userId, ms.Last().Id, mon);
            return _mapper.Map<IEnumerable<Notification>, IEnumerable<DataBaseSourse>>(ntfs);
        }

        public async Task ReadAsync(string userId, string last) {
            Notification notification = last != null ? await _notificationGR.FindAsync(last) : null;
            if (notification == null)
                throw new ClientException("inc-ntf-id", last);
            var ms = _notificationGR.GetDbSet().Where((x) => x.UserId == userId && x.Date >= notification.Date && !x.Read).ToArray().Select((x) => { x.Read = true; return x; }).OrderByDescending((x) => x.Date);

            _notificationGR.GetDbContext().UpdateRange(ms);
            var mon = await _notificationGR.GetDbContext().SaveChangesAsync();
            if (mon > 0 && ms.Count() > 0)
                await ReadAsync(userId, ms.Last().Id, mon);
        }

        public async Task<int> GetCountUnreadAsync(string userId)
            => await _notificationGR.CountWhereAsync((x) => x.UserId == userId && !x.Read);

        public async Task ReadAsync(string userId, string last, int count)
            => await _notificationService.NotifyAsync(userId, new OnReadSource { Last = last, Count = count }, methodReadName);

    }
}