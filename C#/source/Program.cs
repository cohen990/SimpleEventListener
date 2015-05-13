namespace Microsoft.ServiceBus.Samples
{
    using System;
    using SessionMessages;

    class Program
    {
        private const string EventHubName = "testeventhub";
        private const int NumberOfPartitions = 16;
        private const string ConnectionString =
            "Endpoint=sb://testeventhub-gb.servicebus.windows.net/;SharedAccessKeyName=Shared;SharedAccessKey=tmfiwHih9tF+n7j12PKB9ckAp5uPL2yhKJtbvIU1dq8=";

        static void Main()
        {
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);

            Manage.CreateEventHub(EventHubName, NumberOfPartitions, namespaceManager);

            var r = new Receiver(EventHubName, ConnectionString);
            r.MessageProcessingWithPartitionDistribution();

            Console.WriteLine("Press enter key to stop worker.");
            Console.ReadLine();
        }
    }
}
