using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.User
{
    public interface ICheckUserServices
    {
        Task<bool> CheckUserExistByEmailAsync(string email);
        Task<bool> CheckUserExistByUserNameAsync(string userName);
        bool CheckPasswordCompatibility(string password, string hashedPassword);
        Task<bool> CheckUserBlockedAsync(int blockerUserId, int blockedUserId);
    }
}
