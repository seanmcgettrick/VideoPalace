using MassTransit;
using VideoPalace.Catalog.Events;
using VideoPalace.Catalog.Service.Entities;
using VideoPalace.Catalog.Service.Services;
using VideoPalace.Common.Contracts;

namespace VideoPalace.Catalog.Service.Data;

public class SeedMovieCatalog : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SeedMovieCatalog(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Movie>>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var existingMovies = await repository.GetAllAsync();

        if (!existingMovies.Any())
        {
            var movies = new List<Movie>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Ghostbusters",
                    Description = "A movie about ghosts and the men who bust them.",
                    Genre = "Comedy",
                    Rating = "PG",
                    ReleaseYear = 1984,
                    CreatedDate = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Avengers: Endgame",
                    Description = "Ant-Man just wants to eat his tacos, but Thanos has other plans.",
                    Genre = "Action",
                    Rating = "PG-13",
                    ReleaseYear = 2019,
                    CreatedDate = DateTimeOffset.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Close Encounters of the Third Kind",
                    Description = "A man receives messages from aliens via mashed potatoes.",
                    Genre = "Sci-Fi",
                    Rating = "PG",
                    ReleaseYear = 1977,
                    CreatedDate = DateTimeOffset.UtcNow
                }
            };

            await repository.BulkCreateAsync(movies);

            foreach (var movie in movies)
                await publishEndpoint.Publish(new CatalogMovieAdded(movie.Id, movie.Title), cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}