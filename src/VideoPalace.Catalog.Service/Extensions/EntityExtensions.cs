using VideoPalace.Catalog.Service.Entities;
using VideoPalace.Catalog.Service.Entities.Dtos;

namespace VideoPalace.Catalog.Service.Extensions;

public static class EntityExtensions
{
    public static MovieDto AsDto(this Movie movie) => new(movie.Id, movie.Title, movie.Description, movie.Genre, movie.Rating, movie.ReleaseYear, movie.CreatedDate);

    public static Movie AsNewEntity(this AddMovieDto addMovieDto) => new Movie
    {
        Id = Guid.NewGuid(),
        Title = addMovieDto.Title,
        Description = addMovieDto.Description,
        Genre = addMovieDto.Genre,
        Rating = addMovieDto.Rating,
        ReleaseYear = addMovieDto.ReleaseYear,
        CreatedDate = DateTimeOffset.UtcNow
    };
}