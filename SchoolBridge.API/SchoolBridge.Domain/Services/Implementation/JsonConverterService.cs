using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SchoolBridge.Domain.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class JsonConverterService : IJsonConverterService
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public JsonConverterService() {
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
        public string ConvertObjectToJson(object obj)
            => JsonConvert.SerializeObject(obj, _jsonSerializerSettings);

    }
}
