using OfflineMessaging.Domain.Dtos.Block;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.Block
{
    public interface ICrudBlockServices
    {
        Task<bool> AddBlockAsync(BlockUserParametersDto parameters);
    }
}
