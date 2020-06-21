using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class PermanentSubscribeDto
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}
