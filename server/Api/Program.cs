using Api.Extensions;
using Api.Filters;
using Api.Middlewares;
using Application.Extensions;
using Infrastructure.Extensions;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

            builder.Services.AddAuthenticationConfigs(builder.Configuration);

            builder.Services.AddApplication();

            builder.Services.AddInfrastructure(builder.Configuration);
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseMiddleware<CultureMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
