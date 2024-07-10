namespace EfUnitTesting.Models;

public class Movie
{
    public int Identifier { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public AgeRating AgeRating { get; set; }
}