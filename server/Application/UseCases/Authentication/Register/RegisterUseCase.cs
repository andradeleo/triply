using Communication.Request;
using Communication.Resources;
using Communication.Response;
using Domain.Entities;
using Domain.Enums;
using Domain.Infrastructure;
using Domain.Infrastructure.Repositories;
using Domain.Security;
using Exception;

namespace Application.UseCases.Authentication.Register
{
    public class RegisterUseCase(IUserReadOnlyRepository userReadOnlyRepository, IUserWriteOnlyRepository userWriteOnlyRepository, IPasswordEncripter passwordEncripter, IUnitOfWork unitOfWork) : IRegisterUseCase
    {
        public async Task<ResponseRegister> Execute(RequestRegister request)
        {
            await Validate(request);

            var userInDatabase = await userReadOnlyRepository.GetUserByEmail(request.Email);

            if (userInDatabase is not null)
            {
                return new ResponseRegister { Message = ResourceMessages.EMAIL_CONFIRMATION_SENT };
            }

            var user = new User { 
                Name = request.Name, 
                Email = request.Email, 
                Password = passwordEncripter.Encrypt(request.Password), 
                Role = Roles.USER, 
                CreatedAt = DateTime.UtcNow, 
                UpdatedAt = DateTime.UtcNow 
            };

            await userWriteOnlyRepository.Insert(user);

            await unitOfWork.Commit();

            return new ResponseRegister { Message = ResourceMessages.EMAIL_CONFIRMATION_SENT };
        }

        private async Task Validate(RequestRegister request)
        {
            var result = new RegisterValidator().Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
