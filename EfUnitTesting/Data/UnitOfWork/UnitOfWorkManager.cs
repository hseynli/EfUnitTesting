namespace EfUnitTesting.Data.UnitOfWork;

public class UnitOfWorkManager : IUnitOfWorkManager
{
    private readonly MoviesContext _context;

    public UnitOfWorkManager(MoviesContext context)
    {
        _context = context;
    }

    private bool _isUnitOfWorkStarted = false;

    public void StartUnitOfWork()
    {
        _isUnitOfWorkStarted = true;
    }
    public bool IsUnitOfWorkStarted => _isUnitOfWorkStarted;

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}