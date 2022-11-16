using VideoPalace.Catalog.Service.Entities;

namespace VideoPalace.Catalog.Service.Services;

public interface IInventoryService
{
    Task<bool> AddMovieToInventoryAsync(Movie movie);
    Task<bool> BulkAddMovieToInventoryAsync(IEnumerable<Movie> movies);
}