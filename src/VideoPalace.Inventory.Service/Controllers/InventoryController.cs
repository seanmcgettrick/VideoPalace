using Microsoft.AspNetCore.Mvc;
using VideoPalace.Common.Contracts;
using VideoPalace.Inventory.Service.Entities;
using VideoPalace.Inventory.Service.Entities.Dtos;
using VideoPalace.Inventory.Service.Extensions;

namespace VideoPalace.Inventory.Service.Controllers;

[ApiController]
[Route("videos")]
public class InventoryController : ControllerBase
{
    private readonly IRepository<Video> _videoRepository;

    public InventoryController(IRepository<Video> repository) => _videoRepository = repository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VideoDto>))]
    public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideos() =>
        Ok((await _videoRepository.GetAllAsync()).Select(video => video.AsDto()));

    [HttpGet("{id:guid}", Name = "GetVideo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VideoDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VideoDto>> GetVideo(Guid id)
    {
        var video = await _videoRepository.GetAsync(id);

        if (video is null)
            return NotFound();

        return Ok(video.AsDto());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddVideo([FromBody] AddVideoDto addVideoDto)
    {
        var video = addVideoDto.AsNewEntity();

        await _videoRepository.CreateAsync(video);

        return CreatedAtRoute(nameof(GetVideo), new { id = video.Id }, video);
    }
}