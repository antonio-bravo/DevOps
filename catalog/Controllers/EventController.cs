using Microsoft.AspNetCore.Mvc;
using GloboTicket.Catalog.Repositories;

namespace GloboTicket.Catalog.Controllers
{
    /// <summary>
    /// Controlador para gestionar eventos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventController> _logger;

        /// <summary>
        /// Constructor para el controlador de eventos.
        /// </summary>
        /// <param name="eventRepository">Repositorio para gestionar eventos.</param>
        /// <param name="logger">Logger para registrar eventos.</param>
        public EventController(IEventRepository eventRepository, ILogger<EventController> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los eventos.
        /// </summary>
        /// <returns>Una lista de eventos.</returns>
        /// <response code="200">Retorna la lista de eventos.</response>
        /// <response code="404">Si no se encuentran eventos.</response>  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventRepository.GetEvents();
            
            if (events == null || !events.Any())
            {
                return NotFound("No events found");
            }
            
            return Ok(events);
        }

        /// <summary>
        /// Crea un nuevo evento.
        /// </summary>
        /// <param name="request">El objeto evento a crear.</param>
        /// <returns>El evento creado.</returns>
        /// <response code="201">Retorna el evento creado.</response>
        /// <response code="400">Si el objeto evento es nulo.</response>  
        /// <response code="500">Si ocurre un error interno del servidor.</response>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var @event = request.ToEvent();
            
            try
            {
                await _eventRepository.Save(@event);
                return CreatedAtAction(nameof(GetEvent), new {id = @event.EventId}, @event);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error saving event");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Obtiene un evento por su ID.
        /// </summary>
        /// <param name="id">El ID del evento a obtener.</param>
        /// <returns>El evento solicitado.</returns>
        /// <response code="200">Retorna el evento solicitado.</response>
        /// <response code="404">Si el evento no se encuentra.</response>  
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
}