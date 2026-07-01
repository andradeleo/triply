using Communication.Request;
using Communication.Resources;
using Communication.Response;
using Domain.Infrastructure.Repositories;
using Domain.Security;
using Exception;

namespace Application.UseCases.Authentication
{
    public class LoginUseCase(IAccessTokenGenerator accessTokenGenerator, IPasswordEncripter passwordEncripter, IUserReadOnlyRepository userReadOnlyRepository) : ILoginUseCase
    {
        public async Task<ResponseLogin> Execute(RequestLogin request)
        {
            await Validate(request);

            var userInDatabase = await userReadOnlyRepository.GetUserByEmail(request.Email) ?? throw new UnauthorizedException(ResourceMessages.INVALID_CREDENTIALS);

            var passwordMatch = passwordEncripter.Verify(request.Password, userInDatabase.Password);

            if (passwordMatch == false)
            {
                throw new UnauthorizedException(ResourceMessages.INVALID_CREDENTIALS);
            }

            if (userInDatabase.ConfirmedEmailAt == null)
            {
                throw new UnauthorizedException(ResourceMessages.EMAIL_NOT_CONFIRMED);
            }

            return new ResponseLogin { Token = accessTokenGenerator.Generate(userInDatabase) };
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