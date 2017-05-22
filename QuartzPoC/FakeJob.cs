using System;
using System.Threading;
using Quartz;

namespace QuartzPoC
{
    [DisallowConcurrentExecution]
    public class FakeJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.Error
                .WriteLine($"{context.JobDetail.Key} Id {context.FireInstanceId} => Start time: {DateTime.UtcNow.ToLongTimeString()}" );

            Thread.Sleep(10 * 1000);

            Console.Error
                .WriteLine($"{context.JobDetail.Key} Id {context.FireInstanceId} => End time: {DateTime.UtcNow.ToLongTimeString()}");
        }
    }
}