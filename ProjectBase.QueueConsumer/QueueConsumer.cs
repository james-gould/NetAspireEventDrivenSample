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
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var fromDb = await _context.WeatherItems.FirstOrDefaultAsync(x => x.Id == int.Parse(message.Body.ToString()));

            _logger.LogInformation($"Id from database: {fromDb?.GeneratedAt}");

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
