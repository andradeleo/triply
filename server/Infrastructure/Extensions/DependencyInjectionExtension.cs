using Domain.Security;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddToken(services, configuration);
        }

        private static void AddToken(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpiryMinutes");
            var jtwKey = configuration.GetValue<string>("Jwt:Key");
            var issuer = configuration.GetValue<string>("Jwt:Issuer");
            var audience = configuration.GetValue<string>("Jwt:Audience");

            services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, jtwKey!, issuer!, audience!));
        }
    }
}
