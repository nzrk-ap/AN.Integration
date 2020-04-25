using System.Threading.Tasks;
using AN.Integration._1C.Messages;
using AN.Integration._1C.Models;
using AN.Integration.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AN.Integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly HttpQueueClient _httpQueueClient;

        public ContactController(HttpQueueClient httpQueueClient)
        {
            _httpQueueClient = httpQueueClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Contact contact)
        {
            if (contact is null)
                return BadRequest($"{nameof(Contact)} is not valid");

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new UpsertMessage<Contact>(contact));
            return StatusCode(statusCode, content);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(Contact contact)
        {
            if (contact is null)
                return BadRequest($"{nameof(Contact)} is not valid");

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new UpsertMessage<Contact>(contact));
            return StatusCode(statusCode, content);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Contact contact)
        {
            if (contact is null)
                return BadRequest($"{nameof(Contact)} is not valid");

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new DeleteMessage<Contact>(contact.Code));
            return StatusCode(statusCode, content);
        }
    }
}