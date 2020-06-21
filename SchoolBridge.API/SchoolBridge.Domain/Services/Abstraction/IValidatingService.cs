using SchoolBridge.Domain.Services.Implementation;
using System;
using System.Collections.Generic;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IValidatingService
    {
        void Validate(string[] attrs, object obj, string objName);
        void Validate<T>(T dto);
        void Validate(Type type, object dto);
        bool IsIssetValidateFunc(string funcId);
        void AddValidateFunc<T>(string funcId, ValidateFunc<T> func);
    }
}
