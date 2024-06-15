using Microsoft.AspNetCore.Mvc;
using GloboTicket.Catalog.Repositories;

namespace GloboTicket.Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventController> _logger;



        public EventController(IEventRepository eventRepository, ILogger<EventController> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventRepository.GetEvents();
            
            if (events == null || !events.Any())
            {
                return NotFound("No events found");
            }
            
            return Ok(events);
    // [HttpGet(Name = "GetEvents")]
    // public async Task<IActionResult> GetAll()
    // {
    //     return Ok(await _eventRepository.GetEvents());
    // }


        public EventController(IEventRepository eventRepository, ILogger<EventController> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los eventos
        /// </summary>
        /// <returns>Una lista de eventos</returns>
        /// <response code="200">Retorna la lista de eventos</response>
        /// <response code="404">Si no se encontraron eventos</response>  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventRepository.GetEvents();
            
            if (events == null || !events.Any())
            {
                return NotFound("No events found");
            }
            
            return Ok(events);
        }


        /// <summary>
        /// Crea un nuevo evento
        /// </summary>
        /// <param name="request">El objeto evento a crear</param>
        /// <returns>El evento creado</returns>
        /// <response code="201">Retorna el evento creado</response>
        /// <response code="400">Si el objeto evento es nulo</response>  
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var @event = request.ToEvent();
            
            try
            {
                await _eventRepository.Save(@event);

                return CreatedAtAction(nameof(GetById), new {id = @event.EventId}, @event);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error saving event");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {        
            var evt = await _eventRepository.GetEventById(id);
            
            if (evt == null)
            {
                return NotFound();
            }
            
            return Ok(evt);
        }
            _logger.LogError(ex, "Error saving event");
            return StatusCode(500, "Internal server error");
        }

        /// <summary>
        /// Obtiene un evento por su ID
        /// </summary>
        /// <param name="id">El ID del evento a obtener</param>
        /// <returns>El evento solicitado</returns>
        /// <response code="200">Retorna el evento solicitado</response>
        /// <response code="404">Si el evento no se encuentra</response>  
        [HttpGet("{id}", Name = "GetEvent")]
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