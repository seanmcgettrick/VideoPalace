namespace VideoPalace.Catalog.Service.Entities.Dtos;

public record AddMovieDto(string Title, string Description, string Genre, string Rating, int ReleaseYear);