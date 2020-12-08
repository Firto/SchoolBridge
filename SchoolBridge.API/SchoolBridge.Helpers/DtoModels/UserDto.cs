using SchoolBridge.Helpers.AddtionalClases.OnlineService;
using System.ComponentModel.DataAnnotations;

namespace SchoolBridge.Helpers.DtoModels
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string Photo { get; set; }
        public OnlineStatus OnlineStatus { get; set; }
        public string OnlineStatusSubscriptionToken { get; set; }
        public bool Banned { get; set; }
    }
}