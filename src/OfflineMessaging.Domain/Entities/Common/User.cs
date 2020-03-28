using OfflineMessaging.Domain.Entities.Base;

namespace OfflineMessaging.Domain.Entities.Common
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
