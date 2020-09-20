using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using server.Service.chat;
using server.ViewModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/v1/message")]
    [Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ChatServiceHub _serviceHub;

        public MessageController(ChatServiceHub serviceHub) =>
            _serviceHub = serviceHub;

        [HttpPost("private")]
        public async Task<ActionResult<UserMessageViewModel>> PostPrivate([FromBody] UserMessageViewModel model)
        {
            if (model.UserOrigin == null || model.UserTarget == null || model.Message == null)
                return BadRequest();
            await _serviceHub.SendPrivateMessage(model.UserTarget.Username, model.Message);

            return Ok("message send");
        }

        [HttpPost]
        public async Task<ActionResult<UserMessageViewModel>> Post([FromBody] UserMessageViewModel model)
        {
            if (model.UserOrigin == null || model.UserTarget == null || model.Message == null)
                return BadRequest();
            await _serviceHub.SendPrivateMessage(model.UserTarget.Username, model.Message);

            return Ok("message send");
        }
    }
}