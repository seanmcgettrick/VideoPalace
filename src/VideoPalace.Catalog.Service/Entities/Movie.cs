using VideoPalace.Common.Contracts;

namespace VideoPalace.Catalog.Service.Entities;

public class Movie : IEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public string Rating { get; set; } = default!;
    public int ReleaseYear { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}

