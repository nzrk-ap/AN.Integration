using System.Threading.Tasks;
using AN.Integration.Models._1C.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AN.Integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        public Task Post(ProductDto product)
        {
            return Task.CompletedTask;
        }

        [HttpPut]
        public Task Put(ProductDto product) {
            return Task.CompletedTask;
        }

        [HttpDelete]
        public Task Delete(string code) {
            return Task.CompletedTask;
        } 
    }
}