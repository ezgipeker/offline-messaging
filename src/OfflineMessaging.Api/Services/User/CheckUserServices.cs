using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Infrastructure.Context;
using OfflineMessaging.Infrastructure.Extensions.Cryptography;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.User
{
    public class CheckUserServices : ICheckUserServices
    {
        private readonly OfflineMessagingContext _context;

        public CheckUserServices(OfflineMessagingContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckUserExistByEmailAsync(string email)
        {
            var result = await _context.Users.AnyAsync(x => x.Email == email);

            return result;
        }

        public async Task<bool> CheckUserExistByUserNameAsync(string userName)
        {
            var result = await _context.Users.AnyAsync(x => x.UserName == userName);

            return result;
        }

        public bool CheckPasswordCompatibility(string password, string hashedPassword) => password.ValidateHash(hashedPassword);

        public async Task<bool> CheckUserBlockedAsync(int blockerUserId, int blockedUserId)
        {
            var result = await _context.Blocks.AnyAsync(x => x.BlockerUserId == blockerUserId && x.BlockedUserId == blockedUserId);

            return result;
        }
    }
}
