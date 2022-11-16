using VideoPalace.Common.Contracts;

namespace VideoPalace.Inventory.Service.Entities;

public class Video : IEntity
{
    public Guid Id { get; set; }
    public Guid CatalogId { get; set; }
    public string Title { get; set; } = default!;
    public int TotalQuantity { get; set; }
    public int AvailableForRent { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}