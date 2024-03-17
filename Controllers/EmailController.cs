using Microsoft.AspNetCore.Mvc;
using Photoforge_Server.Models;
using Photoforge_Server.Services;

namespace Photoforge_Server.Controllers
{
    [ApiController]
    [Route("email")]
    public class EmailController : Controller
    {

        private readonly IMailService _mailService;

        public EmailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            return Ok();
        }

    }
}
