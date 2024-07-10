namespace EfUnitTesting.Data.UnitOfWork;

public interface IUnitOfWorkManager
{
    void StartUnitOfWork();
    bool IsUnitOfWorkStarted { get; }
    Task<int> SaveChangesAsync();
}
