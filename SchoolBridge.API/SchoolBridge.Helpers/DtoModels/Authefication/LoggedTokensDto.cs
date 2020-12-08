using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels.Authefication
{
    public class LoggedTokensDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}
