using Domain.Entities;

namespace Domain.Security
{
    public interface IAccessTokenGenerator
    {
        string Generate(User user);
    }
}
