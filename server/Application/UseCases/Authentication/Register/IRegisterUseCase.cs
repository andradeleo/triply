using Communication.Request;
using Communication.Response;

namespace Application.UseCases.Authentication.Register
{
    public interface IRegisterUseCase
    {
        Task<ResponseRegister> Execute(RequestRegister request);
    }
}
