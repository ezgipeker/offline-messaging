using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfflineMessaging.Api.Services.User;
using OfflineMessaging.Domain.Dtos.User;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Controllers.User
{
    /// <summary>
    /// User Controller
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        /// ctor
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// User register (AllowAnonymous. Do not send token.)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST user/register
        ///     {
        ///         "UserName" : "ezgipeker",
        ///         "FirstName": "Ezgi",
        ///         "LastName": "Peker",
        ///         "Email": "ezgi.peker.6@gmail.com",
        ///         "Password": "Password123"
        ///      }
        /// </remarks>
        /// <param name="parameters"></param>
        /// <returns>Register response</returns>
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

        /// <summary>
        /// User login (AllowAnonymous. Do not send token.)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST user/login
        ///     {
        ///         "UserName" : "ezgipeker",
        ///         "Password": "Password123"
        ///     }
        /// </remarks>
        /// <param name="parameters"></param>
        /// <returns>Login response and token</returns>
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
