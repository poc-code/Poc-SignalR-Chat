using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace server.Controllers
{
    [Route("api/v1/IsReady")]
    [ApiController]
    [Produces("application/json")]
    public class IsReadyController : ControllerBase
    {
        [HttpGet]
        public IActionResult GET()
        {
            string url = HttpContext.Request.Host.Value;
            // http://localhost:1302/TESTERS/Default6.aspx


            return Ok(new {
                Server = "Is Ready",
                Chat = $"http://{url}/chat1.html",
                Streaming = $"http://{url}/streaming.html"
            });
        }
    }
}