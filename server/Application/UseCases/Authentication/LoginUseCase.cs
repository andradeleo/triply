using Communication.Request;
using Communication.Response;
using Domain.Entities;
using Domain.Enums;
using Domain.Security;
using Exception;

namespace Application.UseCases.Authentication
{
    public class LoginUseCase(IAccessTokenGenerator tokenService, IPasswordEncripter passwordEncripter) : ILoginUseCase
    {
        private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;

        public async Task<ResponseLogin> Execute(RequestLogin request)
        {
            await Validate(request);

            if (request.Email != "admin@admin.com" || request.Password != "123456")
            {
                throw new UnauthorizedException("Email ou senha inválidos");
            }

            var user = new User
            {
                Email = request.Email,
                Password = _passwordEncripter.Encrypt(request.Password),
                Role = Roles.ADMIN
            };

            return new ResponseLogin { Token = tokenService.Generate(user) };
        }

        private async Task Validate(RequestLogin request)
        {
            var result = new LoginValidator().Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}