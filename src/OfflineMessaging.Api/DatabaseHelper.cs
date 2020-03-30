using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfflineMessaging.Domain.Entities;
using OfflineMessaging.Infrastructure.Context;
using System;
using System.Linq;

namespace OfflineMessaging.Api
{
    public static class DatabaseHelper
    {
        public static void PrepareDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<OfflineMessagingContext>());
            }
        }

        public static void SeedData(OfflineMessagingContext context)
        {
            context.Database.Migrate();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { CreateDate = DateTime.Now, UserName = "testuser", FirstName = "Test", LastName = "User", Email = "test@test.com", Password = "CKBgEDtlvGR49dBEUN3uwHNmK42PKlxEgdJmwRD7IMQ=ærM1tqCXhDv+BgMz0AdOuOA==" },
                    new User { CreateDate = DateTime.Now, UserName = "dummyuser", FirstName = "Dummy", LastName = "User", Email = "dummy@test.com", Password = "vEGeyhr6wIMinnnxWyisXiCLSaPAHynnuCD+fCCY2tU=æbYqjmEs6QWHl9xh1YpzOiA==" });

                context.SaveChanges();
            }
        }
    }
}
