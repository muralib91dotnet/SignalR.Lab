using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Lab.Web.Hubs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalR.Lab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public static List<string> Source { get; set; } = new List<string>();

        private IHubContext<ValuesHub> _hubContext;

        public ValuesController(IHubContext<ValuesHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Source;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Source[id];
        }

        // POST api/values
        [HttpPost]
        [Route("addvalue")]
        public async void Post([FromBody] string value)
        {
            Source.Add(value);
            await _hubContext.Clients.All.SendAsync("Add", value);
        }

        // PUT api/values/5
        [HttpPut]
        [Route("editvalue/{id}")]
        public void Put(int id, [FromBody] string value)
        {
            Source[id] = value;
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("removevalue/{id}")]
        public async void Delete(int id)
        {
            var item = Source[id];
            Source.Remove(item);
            await _hubContext.Clients.All.SendAsync("Delete", item);
        }
    }
}
