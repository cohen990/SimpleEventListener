namespace Microsoft.ServiceBus.Samples
{
    using Messaging;
    using System;


    public class Manage
    {
        public static void CreateEventHub(string eventHubName, int numberOfPartitions, NamespaceManager manager)
        {
            try
            {
                // Create the Event Hub
                Console.WriteLine("Creating Event Hub...");
                var ehd = new EventHubDescription(eventHubName) { PartitionCount = numberOfPartitions };
                manager.CreateEventHubIfNotExistsAsync(ehd).Wait();
            }
            catch (AggregateException agexp)
            {
                Console.WriteLine(agexp.Flatten());
            }
        }
    }
}
