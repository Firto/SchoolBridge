using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IJsonConverterService: IMyService
    {
        string ConvertObjectToJson(object obj);
    }
}
