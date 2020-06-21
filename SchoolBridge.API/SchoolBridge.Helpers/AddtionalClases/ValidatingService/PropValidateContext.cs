using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.ValidatingService
{
    public class PropValidateContext
    {
        public IServiceProvider SeriviceProvider { get; private set; }
        public Type TypeDto { get; private set; }
        public object Dto { get; private set; }
        public string PropName { get; set; }
        public List<string> Valid { get; set; }

        public PropValidateContext(IServiceProvider serviceProvider, Type typeDto, object dto) {
            SeriviceProvider = serviceProvider;
            TypeDto = typeDto;
            Dto = dto;
        }
    }
}
