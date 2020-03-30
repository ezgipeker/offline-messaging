using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfflineMessaging.Api.Services.User;
using OfflineMessaging.Domain.Dtos.User;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// User register
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(UserRegisterResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody]UserDto parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.UserName) || string.IsNullOrWhiteSpace(parameters.Email) || string.IsNullOrWhiteSpace(parameters.Password))
            {
                Log.ForContext<UserController>().Error("{method} parameters is not valid! Parameters: {@parameters}", nameof(RegisterAsync), parameters);
                return BadRequest("Lütfen zorunlu alanları doldurunuz.");
            }

            var response = await _userServices.RegisterAsync(parameters);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(UserLoginResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginAsync([FromBody]UserLoginParametersDto parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.UserName) || string.IsNullOrWhiteSpace(parameters.Password))
            {
                Log.ForContext<UserController>().Error("{method} parameters is not valid! Parameters: {@parameters}", nameof(LoginAsync), parameters);
                return BadRequest("Lütfen zorunlu alanları doldurunuz.");
            }

            var response = await _userServices.LoginAsync(parameters);

            return Ok(response);
        }
    }
}
