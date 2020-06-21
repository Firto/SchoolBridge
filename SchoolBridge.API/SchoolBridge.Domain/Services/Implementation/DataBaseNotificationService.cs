using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class DataBaseNotificationService<AUser> : IDataBaseNotificationService<AUser> where AUser : AuthUser
    {
        private readonly INotificationService<AUser> _notificationService;
        private readonly IGenericRepository<Notification<AUser>> _notificationGR;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly IMapper _mapper;

        public DataBaseNotificationService(INotificationService<AUser> notificationService,
                                    IGenericRepository<Notification<AUser>> notificationGR,
                                     IMapper mapper,
                                     ClientErrorManager clientErrorManager)
        {
            _notificationService = notificationService;
            _notificationGR = notificationGR;
            _mapper = mapper;
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            if (!clientErrorManager.IsIssetErrors("DataBaseNotification"))
                clientErrorManager.AddErrors(new ClientErrors("DataBaseNotification",
                    new Dictionary<string, ClientError>
                    {
                        {"inc-ntf-id", new ClientError("Incorrect notification id!")}
                    }
                ));
        }

        private string ObjectToBase64String(object obj)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, _jsonSerializerSettings)));

        public async Task Notify(AUser usr, string type, IDataBaseNotificationSourse sourse)
        {
            await _notificationService.Notify(usr, "dataBase", _mapper.Map<Notification<AUser>, DataBaseSourse>(
                await _notificationGR.CreateAsync(
                    new Notification<AUser>
                    {
                        Date = DateTime.Now,
                        Base64Sourse = ObjectToBase64String(sourse),
                        Type = type,
                        UserId = usr.Id
                    }
                ))
            );
        }

        public async Task Notify(AUser usr, string tokenId, string type, IDataBaseNotificationSourse sourse)
        {
            await _notificationService.Notify(usr, tokenId, "dataBase", _mapper.Map<Notification<AUser>, DataBaseSourse>(
                await _notificationGR.CreateAsync(
                    new Notification<AUser>
                    {
                        Date = DateTime.Now,
                        Base64Sourse = ObjectToBase64String(sourse),
                        Type = type,
                        UserId = usr.Id
                    }
                ))
            );
        }

        public async Task Notify(AUser[] usrs, string type, IDataBaseNotificationSourse sourse)
        {
            var template = new Notification<AUser>
            {
                Date = DateTime.Now,
                Base64Sourse = ObjectToBase64String(sourse),
                Type = type
            };
            var outTemplate = _mapper.Map<Notification<AUser>, DataBaseSourse>(template);
            Notification<AUser> temp;
            var notifications = usrs.Select(x =>
            {
                temp = (Notification<AUser>)template.Clone();
                temp.User = x;
                temp.UserId = x.Id;
                return temp;
            });
            (await _notificationGR.CreateAsync(notifications)).ForEach(async (x) =>
                    await _notificationService.Notify(x.User, "dataBase", outTemplate)
            );
        }

        public async Task NotifyNoBase(AUser usr, string type, IDataBaseNotificationSourse sourse)
        {
            await _notificationService.Notify(usr, "dataBase",
                new DataBaseSourse
                {
                    Date = DateTime.Now,
                    Base64Sourse = ObjectToBase64String(sourse),
                    Type = type
                }
            );
        }

        public async Task NotifyNoBase(AUser usr, string tokenId, string type, IDataBaseNotificationSourse sourse)
        {
            await _notificationService.Notify(usr, tokenId, "dataBase", 
                new DataBaseSourse
                {
                    Date = DateTime.Now,
                    Base64Sourse = ObjectToBase64String(sourse),
                    Type = type
                }
            );
        }

        public async Task NotifyNoBase(AUser[] usrs, string type, IDataBaseNotificationSourse sourse)
        {
            var template = new DataBaseSourse
            {
                Date = DateTime.Now,
                Base64Sourse = ObjectToBase64String(sourse),
                Type = type
            };
            await _notificationService.Notify(usrs, "dataBase", template);
        }

        public async Task<IEnumerable<DataBaseSourse>> Get(AUser usr, string last, int count = 20)
        {
            Notification<AUser> notification = last != null ? await _notificationGR.FindAsync(last) : null;
            if (last != null && notification == null)
                throw new ClientException("inc-ntf-id", last);
            return _mapper.Map<IEnumerable<Notification<AUser>>, IEnumerable<DataBaseSourse>>(_notificationGR.GetDbSet().Where((x) => x.UserId == usr.Id && (notification == null || x.Date < notification.Date)).OrderByDescending((x) => x.Date).Take(count).ToArray());
        }

        public async Task<IEnumerable<DataBaseSourse>> GetAndRead(AUser usr, string last, int count = 20)
        {
            Notification<AUser> notification = last != null ? await _notificationGR.FindAsync(last) : null;
            if (last != null && notification == null)
                throw new ClientException("inc-ntf-id", last);
            var ntfs = _notificationGR.GetDbSet()
                                                   .Where((x) => x.UserId == usr.Id && (notification == null || x.Date < notification.Date))
                                                   .OrderByDescending((x) => x.Date).Take(count)
                                                   .AsEnumerable();
            var ms = ntfs.Where((x) => x.Read == false).Select((x) => { x.Read = true; return x; });
            _notificationGR.GetDbContext().UpdateRange(ms);
            var mon = await _notificationGR.GetDbContext().SaveChangesAsync();
            if (mon > 0)
                await _notificationService.Read(usr, ms.Last().Id, mon);
            return _mapper.Map<IEnumerable<Notification<AUser>>, IEnumerable<DataBaseSourse>>(ntfs);
        }

        public async Task Read(AUser usr, string last) {
            Notification<AUser> notification = last != null ? await _notificationGR.FindAsync(last) : null;
            if (notification == null)
                throw new ClientException("inc-ntf-id", last);
            var ms = _notificationGR.GetDbSet().Where((x) => x.UserId == usr.Id && x.Date >= notification.Date && !x.Read).ToArray().Select((x) => { x.Read = true; return x; }).OrderByDescending((x) => x.Date);

            _notificationGR.GetDbContext().UpdateRange(ms);
            var mon = await _notificationGR.GetDbContext().SaveChangesAsync();
            if (mon > 0 && ms.Count() > 0)
                await _notificationService.Read(usr, ms.Last().Id, mon);
        }

        public async Task<int> GetCountUnread(AUser usr)
            => await _notificationGR.CountWhereAsync((x) => x.UserId == usr.Id && !x.Read);

    }
}