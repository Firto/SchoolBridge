using System.Collections.Generic;

namespace SchoolBridge.Helpers.DtoModels.ClientErrors.Info
{
    public class ClientErrorsInfoDto 
    {
        public string Name { get; set; }
        public IDictionary<string, ClientErrorInfoDto> Errors { get; set; }
        public IEnumerable<ClientErrorsInfoDto> Dictionaries { get; set; }
    }
}