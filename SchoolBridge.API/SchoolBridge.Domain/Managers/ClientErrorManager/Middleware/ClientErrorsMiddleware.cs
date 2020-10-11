using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Managers.CClientErrorManager.Middleware
{
    public class ClientErrorsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ClientErrorManager _clientErrorManager;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public ClientErrorsMiddleware(RequestDelegate next, ClientErrorManager clientErrorManager)
        {
            _next = next;
            _clientErrorManager = clientErrorManager;
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ClientException ex)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(_clientErrorManager.MapClientErrorDtoToResultDto(ex.Id, ex.AdditionalInfo), _jsonSerializerSettings));
            }
            /*catch (NullReferenceException) {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(_clientErrorManager.MapClientErrorDtoToResultDto("v-dto-invalid"), _jsonSerializerSettings));
            }*/
        }
    }
}