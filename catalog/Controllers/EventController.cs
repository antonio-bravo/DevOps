using Microsoft.AspNetCore.Mvc;
using GloboTicket.Catalog.Repositories;

namespace GloboTicket.Catalog.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventRepository _eventRepository;

    private static int callcounter = 0;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventRepository eventRepository, ILogger<EventController> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    // [HttpGet(Name = "GetEvents")]
    // public async Task<IActionResult> GetAll()
    // {
    //     return Ok(await _eventRepository.GetEvents());
    // }

    [HttpGet(Name = "GetEvents")]
    public async Task<IActionResult> GetAll()
    {
        var events = await _eventRepository.GetEvents();
        
        if (events == null || !events.Any())
        {
            return NotFound("No events found");
        }
        
        return Ok(events);
    }
    // [HttpPost]
    // public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    // {
    //     if(!request.IsValid)
    //         return BadRequest();
    
    //     var @event = request.ToEvent();
    
    //     try
    //     {
    //         await _eventRepository.Save(@event);
    //         return CreatedAtRoute("GetById", new {id = @event.EventId}, request);
    //     }
    //     catch(Exception ex)
    //     {
    //         _logger.LogError(ex, "Error saving event");
    //         return StatusCode(500);
    //     }
    // }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var @event = request.ToEvent();
        
        try
        {
            await _eventRepository.Save(@event);
            return CreatedAtRoute("GetEvents", new {id = @event.EventId}, @event);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error saving event");
            return StatusCode(500, "Internal server error");
        }
    }
    // [HttpGet("{id}", Name = "GetById")]
    // public async Task<IActionResult> GetById(Guid id)
    // {        
    //     var evt = await _eventRepository.GetEventById(id);
    //     return Ok(evt);
    // }
    [HttpGet("{id}", Name = "GetEvent")]
    public async Task<IActionResult> GetEvent(Guid id)
    {        
        var evt = await _eventRepository.GetEventById(id);
        
        if (evt == null)
        {
            return NotFound();
        }
        
        return Ok(evt);
    }
}
