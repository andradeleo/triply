using Communication.Request;
using Communication.Response;

namespace Application.UseCases.Authentication.Login
{
    public interface ILoginUseCase
    {
        Task<ResponseLogin> Execute(RequestLogin request);
    }
}
