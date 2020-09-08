using Newtonsoft.Json;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.ProgramStatusService;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class ProgramStatusService : IProgramStatusService
    {
        private ProgramStatusServiceConfiguration _configuration = null;
        private ProgramStatus _status = null;
        public ProgramStatus Status { get => _status; set {
                _status = value;
                if (_configuration.CurrentStatusPath != null)
                    System.IO.File.WriteAllText(_configuration.CurrentStatusPath, JsonConvert.SerializeObject(_status));
        } }

        public ProgramStatusService(ProgramStatusServiceConfiguration configuration) {
            _configuration = configuration;

            if (_configuration.CurrentStatusPath != null && System.IO.File.Exists(_configuration.CurrentStatusPath))
                _status = JsonConvert.DeserializeObject<ProgramStatus>(System.IO.File.ReadAllText(_configuration.CurrentStatusPath));
            else if (_configuration.DefaultStatus != null) Status = _configuration.DefaultStatus;
            else Status = new ProgramStatus();
        }

    }
}
