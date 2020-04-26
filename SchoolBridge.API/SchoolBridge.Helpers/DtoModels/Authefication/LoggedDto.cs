using SchoolBridge.Helpers.AddtionalClases.NotificationService;
using System.Collections.Generic;

namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class LoggedDto
    {
        public LoggedTokensDto Tokens { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Panels { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public IEnumerable<DataBaseSourse> Notifications { get; set; }
    }
}