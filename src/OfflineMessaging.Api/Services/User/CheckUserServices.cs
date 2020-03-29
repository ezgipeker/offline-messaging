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
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> CheckUserExistByUserNameAsync(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName);
        }

        public bool CheckPasswordCompatibility(string password, string hashedPassword) => password.ValidateHash(hashedPassword);
    }
}
