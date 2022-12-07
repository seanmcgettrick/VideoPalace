using MassTransit;
using VideoPalace.Catalog.Events;
using VideoPalace.Common.Contracts;
using VideoPalace.Inventory.Service.Entities;

namespace VideoPalace.Inventory.Service.Consumers;

public class CatalogMovieAddedConsumer : IConsumer<CatalogMovieAdded>
{
    private readonly IRepository<Video> _videoRepository;

    public CatalogMovieAddedConsumer(IRepository<Video> videoRepository) => _videoRepository = videoRepository;

    public async Task Consume(ConsumeContext<CatalogMovieAdded> context)
    {
        var (catalogId, title) = context.Message;

        var existingMovie = await _videoRepository.GetAsync(v => v.CatalogId == catalogId);

        if (existingMovie is not null)
            return;

        var video = new Video
        {
            Id = Guid.NewGuid(),
            CatalogId = catalogId,
            Title = title,
            TotalQuantity = 1,
            AvailableForRent = 1,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await _videoRepository.CreateAsync(video);
    }
}