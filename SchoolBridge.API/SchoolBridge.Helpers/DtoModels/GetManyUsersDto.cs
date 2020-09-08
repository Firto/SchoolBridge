using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class GetManyUsersDto
    {
        [PropValid("str-input")]
        public IEnumerable<string> GetTokens { get; set; }
    }
}
