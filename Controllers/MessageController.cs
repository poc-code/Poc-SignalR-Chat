using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using server.Interfaces;
using server.Service.chat;
using server.ViewModel;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/v1/message")]
    [Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IChatServiceHub _serviceHub;
        private readonly IHubContext<ChatServiceHub> _context;


        public MessageController(IChatServiceHub serviceHub,IHubContext<ChatServiceHub> context)
        {
            _serviceHub = serviceHub;
            _context = context;
        }

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
            if (model.UserOrigin == null || model.Message == null)
                return BadRequest();
            await _context.Clients.All.SendAsync("ReceiveMessage", model.UserOrigin.Username, model.Message);

            return Ok("message send");
        }


    }
}