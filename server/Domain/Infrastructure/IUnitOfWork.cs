namespace Domain.Infrastructure
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
