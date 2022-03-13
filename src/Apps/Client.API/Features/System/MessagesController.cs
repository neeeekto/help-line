using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Authorization;
using HelpLine.Apps.Client.API.Features.System.Models;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.System
{
    [ApiController]
    [Route("v1/system/messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IStorage<Message> _storage;

        public MessagesController(IStorage<Message> storage)
        {
            _storage = storage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Message>>> Get([FromQuery] bool all = false)
        {
            var result = await _storage.GetList();
            return Ok(result.Where(x =>
                all || ((x.Data.ShowAt == null || x.Data.ShowAt <= DateTime.UtcNow) &&
                        (x.Data.HideAt == null || x.Data.HideAt > DateTime.UtcNow))).ToArray());
        }

        [HttpPost]
        [Route("")]
        [HasPermissions(SystemPermissions.System)]
        public async Task<ActionResult<Message>> Add(MessageData data)
        {
            var message = new Message()
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.UtcNow,
                Data = data
            };
            await _storage.Set(message.Id, message);
            return Ok(message);
        }

        [HttpPatch]
        [Route("{messageId:guid}")]
        [HasPermissions(SystemPermissions.System)]
        public async Task<ActionResult<Message>> Update(MessageData data, Guid messageId)
        {
            var message = await _storage.Get(messageId);
            message.Data = data;
            await _storage.Set(message.Id, message);
            return Ok(message);
        }

        [HttpDelete]
        [Route("{messageId:guid}")]
        [HasPermissions(SystemPermissions.System)]
        public async Task<ActionResult> Delete(Guid messageId)
        {
            await _storage.RemoveOne(messageId);
            return Ok();
        }
    }
}
