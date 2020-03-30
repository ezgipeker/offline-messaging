using Microsoft.EntityFrameworkCore;
using System;

namespace OfflineMessaging.Api.ServicesTests.Base.Helpers
{
    public static class TestHelper
    {
        public static TDbContext GetInMemoryDbContext<TDbContext>() where TDbContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseInMemoryDatabase("OfflineMessaging");
            var context = Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options) as TDbContext;
            return context;
        }
    }
}
