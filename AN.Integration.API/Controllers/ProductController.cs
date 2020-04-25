using System.Threading.Tasks;
using AN.Integration._1C.Messages;
using AN.Integration.API.Services;
using AN.Integration.Models._1C.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AN.Integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly HttpQueueClient _httpQueueClient;

        public ProductController(HttpQueueClient httpQueueClient)
        {
            _httpQueueClient = httpQueueClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            if (product is null)
                return BadRequest($"{nameof(Product)} is not valid");

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new UpsertMessage<Product>(product));
            return StatusCode(statusCode, content);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(Product product)
        {
            if (product is null)
                return BadRequest($"{nameof(Product)} is not valid");

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new UpsertMessage<Product>(product));
            return StatusCode(statusCode, content);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Product product)
        {
            if (product is null)
                return BadRequest($"{nameof(Product)} is not valid");

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new DeleteMessage<Product>(product.Code));
            return StatusCode(statusCode, content);
        }
    }
}