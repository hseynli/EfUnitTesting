using EfUnitTesting.Data;
using EfUnitTesting.Data.UnitOfWork;
using EfUnitTesting.Models;
using Microsoft.EntityFrameworkCore;

namespace EfUnitTesting.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly MoviesContext _context;
    private readonly IUnitOfWorkManager _uowManager;

    public GenreRepository(MoviesContext context, IUnitOfWorkManager uowManager)
    {
        _context = context;
        _uowManager = uowManager;
    }

    public async Task<IEnumerable<Genre>> GetAll()
    {
        return await _context.Genres.ToListAsync();
    }

    private static readonly Func<MoviesContext, int, Genre?> CompiledQuery = EF.CompileQuery((MoviesContext context, int genreId) =>
                                            context.Genres.FirstOrDefault(genre => genre.Id == genreId));

    public async Task<Genre?> Get(int id) => await Task.FromResult(CompiledQuery(_context, id));

    public async Task<Genre> Create(Genre genre)
    {
        await _context.Genres.AddAsync(genre);

        _context.Entry(genre)
            .Property<string?>("TenantId")
            .CurrentValue = _context.TenantId;

        if (!_uowManager.IsUnitOfWorkStarted)
            await _context.SaveChangesAsync();

        return genre;
    }

    public async Task<Genre?> Update(int id, Genre genre)
    {
        var existingGenre = await _context.Genres.FindAsync(id);

        if (existingGenre is null)
            return null;

        existingGenre.Name = genre.Name;

        if (!_uowManager.IsUnitOfWorkStarted)
            await _context.SaveChangesAsync();

        return existingGenre;
    }

    public async Task<bool> Delete(int id)
    {
        var existingGenre = await _context.Genres.FindAsync(id);

        if (existingGenre is null)
            return false;

        _context.Genres.Remove(existingGenre);

        if (!_uowManager.IsUnitOfWorkStarted)
            await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Genre>> GetAllFromQuery()
    {
        var minimumGenreId = 2;

        var genres = await _context.Genres
            .FromSql($"SELECT * FROM [dbo].[Genres] WHERE ID >= {minimumGenreId}")
            .Where(genre => genre.Name != "Comedy")
            .ToListAsync();

        return genres;
    }

    public async Task<IEnumerable<GenreName>> GetNames()
    {
        var names = await _context.Database
            .SqlQuery<GenreName>($"SELECT Name FROM dbo.Genres")
            .ToListAsync();

        return names;
    }
}