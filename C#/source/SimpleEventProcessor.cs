namespace Microsoft.ServiceBus.Samples
{
    using System.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Messaging;

    public class SimpleEventProcessor : IEventProcessor
    {
        PartitionContext _partitionContext;
        Stopwatch _checkpointStopWatch;

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine("SimpleEventProcessor initialize.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset);
            _partitionContext = context;
            _checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> events)
        {
            try
            {
                foreach (EventData eventData in events)
                {
                    string newData = DeserializeEventData(eventData);

                    Console.WriteLine("Message received.  Partition: '{0}', Data: '{1}'", _partitionContext.Lease.PartitionId, newData);
                }

                //Call checkpoint every 5 minutes, so that worker can resume processing from the 5 minutes back if it restarts.
                if (_checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
                {
                    await context.CheckpointAsync();
                    _checkpointStopWatch.Restart();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error in processing: " + exp.Message);
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("Processor Shuting Down.  Partition '{0}', Reason: '{1}'.", _partitionContext.Lease.PartitionId, reason.ToString());
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        string DeserializeEventData(EventData eventData)
        {
            return Encoding.UTF8.GetString(eventData.GetBytes());
        }
    }
}
