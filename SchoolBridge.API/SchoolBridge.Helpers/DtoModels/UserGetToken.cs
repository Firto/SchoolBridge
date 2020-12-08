using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class UserGetToken
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}
