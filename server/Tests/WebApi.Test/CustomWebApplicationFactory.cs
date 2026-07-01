using Domain.Entities;
using Domain.Enums;
using Domain.Security;
using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Api.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test").ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<DatabaseContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);
            });
        }

        private void StartDatabase(DatabaseContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            var admin = new User
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Email = "admin@admin.com",
                Password = passwordEncripter.Encrypt("123456"),
                Role = Roles.ADMIN,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ConfirmedEmailAt = DateTime.UtcNow
            };

            var pending = new User
            {
                Id = Guid.NewGuid(),
                Name = "Pending",
                Email = "pending@pending.com",
                Password = passwordEncripter.Encrypt("123456"),
                Role = Roles.USER,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ConfirmedEmailAt = null
            };

            dbContext.Users.AddRange(admin, pending);
            dbContext.SaveChanges();
        }
    }
}
