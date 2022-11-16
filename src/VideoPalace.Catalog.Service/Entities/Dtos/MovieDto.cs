namespace VideoPalace.Catalog.Service.Entities.Dtos;

public record MovieDto(Guid Id, string Title, string Description, string Genre, string Rating, int ReleaseYear, DateTimeOffset CreatedDate);