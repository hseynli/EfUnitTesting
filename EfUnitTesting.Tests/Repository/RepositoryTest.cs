using EfUnitTesting.Controllers;
using EfUnitTesting.Models;
using EfUnitTesting.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace EfUnitTesting.Tests.Repository;

public class RepositoryTest
{
    [Fact]
    public async Task IfGenreExists_ReturnsGenre()
    {
        // Arrange
        IGenreRepository repository = Substitute.For<IGenreRepository>();
        repository.Get(2)!.Returns(Task.FromResult(new Genre { Id = 2, Name = "Action" }));
        GenresWithRepositoryController controller = new GenresWithRepositoryController(repository);

        // Act
        var response = await controller.Get(2);
        var okResult = response as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Action", (okResult.Value as Genre)?.Name);
        await repository.Received().Get(2);
    }
}