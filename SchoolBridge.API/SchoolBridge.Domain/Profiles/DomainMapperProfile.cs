using AutoMapper;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using SchoolBridge.Helpers.DtoModels;

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

            CreateMap(typeof(Notification<>), typeof(DataBaseSourse));

            CreateMap(typeof(Notification<>), typeof(NotificationDto));
        }
    }
}