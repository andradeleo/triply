using Application.UseCases.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ILoginUseCase, LoginUseCase>();
        }
    }
}
