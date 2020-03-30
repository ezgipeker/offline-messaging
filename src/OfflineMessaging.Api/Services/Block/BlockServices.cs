using OfflineMessaging.Api.Services.User;
using OfflineMessaging.Domain.Dtos.Block;
using OfflineMessaging.Infrastructure.Context;
using Serilog;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.Block
{
    public class BlockServices : IBlockServices
    {
        private readonly OfflineMessagingContext _context;
        private readonly ICrudUserServices _crudUserServices;
        private readonly ICrudBlockServices _crudBlockServices;

        public BlockServices(OfflineMessagingContext context, ICrudUserServices crudUserServices, ICrudBlockServices crudBlockServices)
        {
            _context = context;
            _crudUserServices = crudUserServices;
            _crudBlockServices = crudBlockServices;
        }

        public async Task<BlockUserResponseDto> BlockUserAsync(BlockUserParametersDto parameters)
        {
            var blockedUser = await _crudUserServices.GetUserByUserNameAsync(parameters.BlockedUserName);
            if (blockedUser == null)
            {
                Log.ForContext<BlockServices>().Error("{method} user not exist! Parameters: {@parameters}", nameof(BlockUserAsync), parameters);
                return new BlockUserResponseDto { Success = false, Message = "Bu kullanıcı adına sahip bir kullanıcı bulunamadı." };
            }

            parameters.BlockedUserId = blockedUser.Id;
            var success = await _crudBlockServices.AddBlockAsync(parameters);

            if (!success)
            {
                Log.ForContext<BlockServices>().Error("{method} AddBlockAsync return success false! Parameters: {@parameters}", nameof(BlockUserAsync), parameters);
                return new BlockUserResponseDto { Success = false, Message = "Bloklama sırasında bir hata oluştu. Lütfen tekrar deneyiniz." };
            }

            Log.ForContext<BlockServices>().Information("{method} finished. User successfully blocked! Parameters: {@parameters}", nameof(BlockUserAsync), parameters);

            return new BlockUserResponseDto { Success = true, Message = "Kullanıcı bloklandı." };
        }
    }
}
