using Domain.Entities;
using Domain.Infrastructure.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(DatabaseContext dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository
    {
        public async Task<User?> GetUserByEmail(string email)
        {
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task Insert(User user)
        {
            await dbContext.Users.AddAsync(user);
        }
    }
}
