using Microsoft.AspNetCore.Mvc;
using VideoPalace.Catalog.Service.Entities;
using VideoPalace.Catalog.Service.Entities.Dtos;
using VideoPalace.Catalog.Service.Extensions;
using VideoPalace.Catalog.Service.Services;
using VideoPalace.Common.Contracts;

namespace VideoPalace.Catalog.Service.Controllers;

[ApiController]
[Route("movies")]
public class CatalogController : ControllerBase
{
    private readonly IRepository<Movie> _movieRepository;
    private readonly IInventoryService _inventoryService;

    public CatalogController(IRepository<Movie> movieRepository, IInventoryService inventoryService)
    {
        _movieRepository = movieRepository;
        _inventoryService = inventoryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MovieDto>))]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies() =>
        Ok((await _movieRepository.GetAllAsync()).Select(movie => movie.AsDto()));

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieDto>> GetMovie(Guid id)
    {
        var movie = await _movieRepository.GetAsync(id);

        if (movie is null)
            return NotFound();

        return Ok(movie.AsDto());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddMovie([FromBody] AddMovieDto addMovieDto)
    {
        var movie = addMovieDto.AsNewEntity();

        await _movieRepository.CreateAsync(movie);

        await _inventoryService.AddMovieToInventoryAsync(movie);

        return CreatedAtRoute(nameof(GetMovie), new { id = movie.Id }, movie);
    }
}