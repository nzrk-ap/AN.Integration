using System.Threading.Tasks;
using AN.Integration.API.Extensions;
using AN.Integration.API.Services;
using AN.Integration.OneC.Messages;
using AN.Integration.OneC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AN.Integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly HttpQueueClient _httpQueueClient;
        private readonly ILogger<Product> _logger;

        public ProductController(HttpQueueClient httpQueueClient,
            ILogger<Product> logger)
        {
            _httpQueueClient = httpQueueClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            _logger.LogIsOk<Product>(product.Code, nameof(Post));

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new UpsertMessage<Product>(product));
            return StatusCode(statusCode, content);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(Product product)
        {
            _logger.LogIsOk<Product>(product.Code, nameof(Patch));

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new UpsertMessage<Product>(product));
            return StatusCode(statusCode, content);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string code)
        {
            _logger.LogIsOk<Product>(code, nameof(Delete));

            var (statusCode, content) = await _httpQueueClient
                .SendMessageAsync(new DeleteMessage<Product>(code));
            return StatusCode(statusCode, content);
        }
    }
}