using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using ProjectBase.Data;
using ProjectBase.Data.Models;

namespace ProjectBase.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private ServiceBusClient _client;
        private ProjectDbContext _context;

        public WeatherForecastController(ServiceBusClient client, ProjectDbContext context)
        {
            _client = client;
            _context = context;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("PublishWeatherItem")]
        public async Task PublicWeatherItemAsync()
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
