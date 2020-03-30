using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfflineMessaging.Api.Services.Block;
using OfflineMessaging.Domain.Dtos.Block;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Controllers.Block
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BlockController : ControllerBase
    {
        private readonly IBlockServices _blockServices;

        public BlockController(IBlockServices blockServices)
        {
            _blockServices = blockServices;
        }

        [HttpPost]
        [Route("user")]
        [ProducesResponseType(typeof(BlockUserResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BlockUserAsync([FromBody]BlockUserParametersDto parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.BlockedUserName))
            {
                Log.ForContext<BlockController>().Error("{method} parameters is not valid! Parameters: {@parameters}", nameof(BlockUserAsync), parameters);
                return BadRequest("Lütfen bloklanacak kullanıcı adını giriniz.");
            }

            parameters.BlockerUserId = int.Parse(User.FindFirst("UserId")?.Value);
            var response = await _blockServices.BlockUserAsync(parameters);

            return Ok(response);
        }
    }
}
