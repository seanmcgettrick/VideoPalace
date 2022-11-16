namespace VideoPalace.Inventory.Service.Entities.Dtos;

public record VideoDto(Guid Id, Guid CatalogId, string Title, int TotalQuantity, int AvailableForRent, DateTimeOffset CreatedDate);