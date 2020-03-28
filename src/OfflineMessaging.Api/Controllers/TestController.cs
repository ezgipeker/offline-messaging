using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfflineMessaging.Api.Services;
using Serilog;

namespace OfflineMessaging.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestServices _testServices;
        public TestController(ITestServices testServices)
        {
            _testServices = testServices;
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {
            var response = _testServices.Test();

            return Ok(response.Token);
        }

        [HttpGet("hello")]
        public IActionResult Hello()
        {
            Log.Information("Hello!");
            return Ok("Ok!");
        }
    }
}
