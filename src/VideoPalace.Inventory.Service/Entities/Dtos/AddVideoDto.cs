namespace VideoPalace.Inventory.Service.Entities.Dtos;

public record AddVideoDto(Guid CatalogId, string Title, int TotalQuantity);