using Application.UseCases.Authentication.Login;
using Application.UseCases.Authentication.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<IRegisterUseCase, RegisterUseCase>();
        }
    }
}
