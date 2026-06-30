using Domain.Infrastructure;

namespace Infrastructure.Database
{
    public class UnitOfWork(DatabaseContext dbContext) : IUnitOfWork
    {
        private readonly DatabaseContext _dbContext = dbContext;

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
