using System.Collections.Generic;

namespace SchoolBridge.Helpers.DtoModels.ClientErrors.Info
{
    public class ClientBaseErrorsInfoDto 
    {
        public IDictionary<string, ClientErrorInfoDto> Errors { get; set; }
        public IEnumerable<ClientBaseErrorsInfoDto> Dictionaries { get; set; }
    }
}