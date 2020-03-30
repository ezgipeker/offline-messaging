using OfflineMessaging.Domain.Dtos.Block;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.Block
{
    public interface IBlockServices
    {
        Task<BlockUserResponseDto> BlockUserAsync(BlockUserParametersDto parameters);
    }
}
