using System;

namespace SchoolBridge.Helpers.AddtionalClases.ValidatingService
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropValidAttribute : Attribute
    {
        public string[] FuncIdsAtributes { get; private set; }

        public PropValidAttribute(params string[] funcIds)
            => FuncIdsAtributes = funcIds;
    }
}
