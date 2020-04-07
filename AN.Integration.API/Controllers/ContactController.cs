using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AN.Integration.Models._1C.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AN.Integration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product product)
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string code)
        {
            return Ok();
        }
    }
}