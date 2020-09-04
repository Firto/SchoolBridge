using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolBridge.Helpers.DtoModels;
using SchoolBridge.Domain.Managers.CClientErrorManager;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/")]
    public class ErrorsController : ControllerBase
    {
        private readonly ClientErrorManager _clientErrorManager;

        public ErrorsController(ClientErrorManager clientErrorManager)
            => _clientErrorManager = clientErrorManager;

        [HttpGet]
        public async Task<ResultDto> Get()
            => new ResultDto { ok = true, result = _clientErrorManager.GetInfo()};
    }
}