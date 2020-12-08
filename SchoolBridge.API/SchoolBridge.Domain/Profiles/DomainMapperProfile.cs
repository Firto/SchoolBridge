using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Entities.Chat;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Helpers.DtoModels.Chat;

namespace SchoolBridge.Domain.Profiles
{
    public class DomainMapperProfile: Profile
    {
        public DomainMapperProfile() {
            CreateMap<User, ProfileDto>().ConvertUsing(t => 
                new ProfileDto
                {
                    Login = t.Login,
                    Email = t.Email,
                }
            );

            CreateMap<Notification, DataBaseSourse>();

            CreateMap<Notification, NotificationDto>();
            CreateMap<DirectChat, DirectChatDto>();
            CreateMap<DirectMessage, MessageDto>().ConvertUsing(t =>
                new MessageDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Type = t.Type,
                    Base64Source = t.Base64Source,
                    Sender = new ShortUserDto { Id = t.SenderId}
                }
            );
        }
    }
}