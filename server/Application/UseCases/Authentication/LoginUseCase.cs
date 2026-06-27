using Communication.Request;
using Communication.Response;
using Domain.Entities;
using Domain.Enums;
using Domain.Security;

namespace Application.UseCases.Authentication
{
    public class LoginUseCase(IAccessTokenGenerator tokenService) : ILoginUseCase
    {
        public async Task<ResponseLogin> Execute(RequestLogin request)
        {
            if (request.Email != "admin@admin.com" || request.Password != "123456")
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Role = Roles.ADMIN
              
            };

            return new ResponseLogin { Token = tokenService.Generate(user) };
        }
    }
}