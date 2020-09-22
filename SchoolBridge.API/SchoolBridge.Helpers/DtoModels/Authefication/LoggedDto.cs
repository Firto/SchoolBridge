using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using System.Collections.Generic;

namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class LoggedDto
    {
        public LoggedTokensDto Tokens { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public int CountUnreadNotifications { get; set; }
    }
}