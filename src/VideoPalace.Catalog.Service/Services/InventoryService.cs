using System.Text;
using VideoPalace.Catalog.Service.Entities;

namespace VideoPalace.Catalog.Service.Services;

public class InventoryService : IInventoryService
{
    private readonly HttpClient _httpClient;

    public InventoryService(HttpClient httpClient) => _httpClient = httpClient;


    public async Task<bool> AddMovieToInventoryAsync(Movie movie)
    {
        var requestJson =
            $$"""
            {
              "catalogId": "{{ movie.Id}} ",
              "title": "{{ movie.Title}} ",
              "totalQuantity": 1
            }
            """ ;

        var request = new HttpRequestMessage(HttpMethod.Post, "videos")
        {
            Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> BulkAddMovieToInventoryAsync(IEnumerable<Movie> movies)
    {
        var success = true;

        foreach (var movie in movies) success = await AddMovieToInventoryAsync(movie);

        return success;
    }
}