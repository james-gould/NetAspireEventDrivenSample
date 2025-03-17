using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.Data;
using ProjectBase.Data.Models;

namespace ProjectBase.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueuePublisherController : ControllerBase
    {
        private ServiceBusClient _client;
        private ProjectDbContext _context;

        public QueuePublisherController(ServiceBusClient client, ProjectDbContext context)
        {
            _client = client;
            _context = context;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("PublishItemToServiceBus")]
        public async Task PublishItemToServiceBusAsync()
        {
            var newItem = new WeatherItem { Reading = Summaries[Random.Shared.Next(Summaries.Length)] };
            _context.WeatherItems.Add(newItem);

            await _context.SaveChangesAsync();

            var sender = _client.CreateSender("consumer");

            var msg = new ServiceBusMessage(newItem.Id.ToString());

            await sender.SendMessageAsync(msg);
        }
    }
}
