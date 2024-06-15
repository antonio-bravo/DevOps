using GloboTicket.Catalog.Controllers;
using GloboTicket.Frontend.Extensions;
using GloboTicket.Frontend.Models.Api;
using Newtonsoft.Json;

namespace GloboTicket.Frontend.Services
{    public class EventCatalogService : IEventCatalogService
    {
        private readonly HttpClient client;

        public EventCatalogService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            var response = await client.GetAsync("event");
            return await response.ReadContentAs<List<Event>>();
        }

        public async Task<Event> GetEventById(Guid id)
        {
            var response = await client.GetAsync($"/api/Event/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error getting event with ID {id}: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var evt = JsonConvert.DeserializeObject<Event>(content);
            return evt;
        }
        public Task CreateEvent(CreateEventRequest createEventRequest) => client.PostAsJsonAsync("event", createEventRequest);
    }
}
