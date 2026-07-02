using Domain.Entities;

namespace Domain.Infrastructure.Repositories
{
    public interface IUserWriteOnlyRepository
    {
        Task Insert(User user);
    }
}
