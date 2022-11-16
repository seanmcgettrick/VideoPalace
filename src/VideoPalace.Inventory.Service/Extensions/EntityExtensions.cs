using VideoPalace.Inventory.Service.Entities;
using VideoPalace.Inventory.Service.Entities.Dtos;

namespace VideoPalace.Inventory.Service.Extensions;

public static class EntityExtensions
{
    public static VideoDto AsDto(this Video video) => new(video.Id, video.CatalogId, video.Title,
        video.TotalQuantity, video.AvailableForRent, video.CreatedDate);

    public static Video AsNewEntity(this AddVideoDto addVideoDto) => new Video
    {
        Id = Guid.NewGuid(),
        CatalogId = addVideoDto.CatalogId,
        Title = addVideoDto.Title,
        TotalQuantity = addVideoDto.TotalQuantity,
        AvailableForRent = addVideoDto.TotalQuantity,
        CreatedDate = DateTimeOffset.UtcNow
    };
}