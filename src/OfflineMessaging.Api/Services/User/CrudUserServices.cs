using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Domain.Dtos.User;
using OfflineMessaging.Infrastructure.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.User
{
    public class CrudUserServices : ICrudUserServices
    {
        private readonly OfflineMessagingContext _context;

        public CrudUserServices(OfflineMessagingContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUserAsync(UserDto parameters)
        {
            await _context.Users.AddAsync(new Domain.Entities.User
            {
                CreateDate = DateTime.Now,
                UserName = parameters.UserName,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                Email = parameters.Email,
                Password = parameters.Password
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Domain.Entities.User> GetUserByUserNameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            return user;
        }

        public Domain.Entities.User GetUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            return user;
        }
    }
}
