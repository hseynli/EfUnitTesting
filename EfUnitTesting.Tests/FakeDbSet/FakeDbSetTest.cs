using EfUnitTesting.Controllers;
using EfUnitTesting.Data;
using EfUnitTesting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace EfUnitTesting.Tests.FakeDbSet;

public class FakeDbSetTest
{
    [Fact]
    public async Task IfGenreExists_ReturnsGenre()
    {
        // Arrange
        var context = CreateFakeDbContext();
        var controller = new GenresController(context);

        // Act
        var response = await controller.Get(2);
        var okResult = response as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Action", (okResult.Value as Genre)?.Name);
        await context.DidNotReceive().SaveChangesAsync();
    }

    private MoviesContext CreateFakeDbContext()
    {
        List<Genre> genres = new()
        {
            new Genre { Id = 1, Name = "Drama"},
            new Genre { Id = 2, Name = "Action"},
            new Genre { Id = 3, Name = "Comedy"}
        };

        MoviesContext context = Substitute.For<MoviesContext>();

        DbSet<Genre> genresSet = genres.AsQueryable().BuildMockDbSet();

        // Because controller uses FindAsync method
        // Don't use this line if controller uses FirstOrDefaultAsync method
        genresSet.FindAsync(2)!.Returns(new ValueTask<Genre>(genres.ElementAt(1)));

        context.Genres.Returns(genresSet);

        return context;
    }
}