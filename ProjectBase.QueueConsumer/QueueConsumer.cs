using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectBase.Data;

namespace ProjectBase_QueueConsumer
{
    public class QueueConsumer
    {
        private readonly ILogger<QueueConsumer> _logger;
        private ProjectDbContext _context;

        public QueueConsumer(ILogger<QueueConsumer> logger, ProjectDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function(nameof(QueueConsumer))]
        public async Task Run(
            [ServiceBusTrigger("consumer", Connection = "projectBus")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            var weatherItemId = int.Parse(message.Body.ToString());
            var weatherItem = await _context.WeatherItems
                .Include(e => e.City)
                .FirstOrDefaultAsync(x => x.WeatherItemId == weatherItemId);

            if (weatherItem is null)
            {
                _logger.LogError($"WeatherItem was null!");
                return;
            }

            _logger.LogInformation($"The forecast is {weatherItem.Reading} in {weatherItem.City.CityName} at {weatherItem.GeneratedAt}");

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
