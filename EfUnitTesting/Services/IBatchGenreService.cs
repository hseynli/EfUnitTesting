using EfUnitTesting.Models;

namespace EfUnitTesting.Services;

public interface IBatchGenreService
{
    Task<IEnumerable<Genre>> CreateGenres(IEnumerable<Genre> genres);
}
