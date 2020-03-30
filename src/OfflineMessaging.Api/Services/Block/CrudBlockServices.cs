using OfflineMessaging.Domain.Dtos.Block;
using OfflineMessaging.Infrastructure.Context;
using System;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.Block
{
    public class CrudBlockServices : ICrudBlockServices
    {
        private readonly OfflineMessagingContext _context;

        public CrudBlockServices(OfflineMessagingContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBlockAsync(BlockUserParametersDto parameters)
        {
            await _context.Blocks.AddAsync(new Domain.Entities.Block
            {
                CreateDate = DateTime.Now,
                BlockerUserId = parameters.BlockerUserId,
                BlockedUserId = parameters.BlockedUserId
            });

            var result = await _context.SaveChangesAsync() > 0;

            return result;
        }
    }
}
