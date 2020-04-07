using System.Threading.Tasks;
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
            await _httpQueueClient.SendMessageAsync(product);
            return Ok();
        }

        [HttpPut]
        public Task Put(ProductDto product)
        {
            return Task.CompletedTask;
        }

        [HttpDelete]
        public Task Delete(string code)
        {
            return Task.CompletedTask;
        }
    }
}