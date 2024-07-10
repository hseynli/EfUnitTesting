using EfUnitTesting.Data.UnitOfWork;
using EfUnitTesting.Models;
using EfUnitTesting.Repositories;

namespace EfUnitTesting.Services;

public class BatchGenreService : IBatchGenreService
{
    private readonly IGenreRepository _repository;
    private readonly IUnitOfWorkManager _uowManager;

    public BatchGenreService(IGenreRepository repository, IUnitOfWorkManager uowManager)
    {
        _repository = repository;
        _uowManager = uowManager;
    }

    public async Task<IEnumerable<Genre>> CreateGenres(IEnumerable<Genre> genres)
    {
        List<Genre> response = new ();
        
        _uowManager.StartUnitOfWork();
        
        foreach (var genre in genres)
        {
            response.Add(await _repository.Create(genre));
        }

        await _uowManager.SaveChangesAsync();
        
        return response;
    }
}