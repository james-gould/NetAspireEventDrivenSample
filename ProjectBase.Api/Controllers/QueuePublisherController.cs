using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var forecast = Summaries[Random.Shared.Next(Summaries.Length)];

            var city = await _context.Cities.FirstAsync();

            var weatherForecast = new WeatherItem
            {
                Reading = forecast,
                City = city
            };

            _context.WeatherItems.Add(weatherForecast);

            await _context.SaveChangesAsync();

            var sender = _client.CreateSender("consumer");

            var msg = new ServiceBusMessage(weatherForecast.WeatherItemId.ToString());

            await sender.SendMessageAsync(msg);
        }
    }
}
