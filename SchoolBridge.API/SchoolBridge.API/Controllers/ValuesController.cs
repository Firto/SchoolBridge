using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.DataBaseNotoficationService;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IWebHostEnvironment _webHostEnvironment;
        IDataBaseNotificationService<User> _dataBaseNotificationManager;
        IGenericRepository<User> _userGR;

        public ValuesController(IWebHostEnvironment webHostEnvironment, 
                                IDataBaseNotificationService<User> dataBaseNotificationManager,
                                IGenericRepository<User> userGR) {
            _webHostEnvironment = webHostEnvironment;
            _dataBaseNotificationManager = dataBaseNotificationManager;
            _userGR = userGR;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            await _dataBaseNotificationManager.Notify(_userGR.GetAll((x) => x.Login == "admin").First(), "test", new MessageNotificationSource { Message = "Baboola" });
            return new string[] { _webHostEnvironment.ContentRootPath, _webHostEnvironment.WebRootPath };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}