using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Mango.MessageBus
{
	public class MessageBus : IMessageBus
	{
        private string connectionString = "Endpoint=sb://mangowebns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=g89Q64WWR1jt4ZWfauKMg0uKRuMdyaIFw+ASbKb2avo=";

        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}

