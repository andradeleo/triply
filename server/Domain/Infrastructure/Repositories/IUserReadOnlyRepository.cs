using Domain.Entities;

namespace Domain.Infrastructure.Repositories
{
    public interface IUserReadOnlyRepository
    {
        Task<User?> GetUserByEmail(string email);
    }
}
