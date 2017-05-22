using System;
using System.Collections.Generic;
using System.Threading;
using Quartz;
using Quartz.Impl;

namespace QuartzPoC
{
    class Program
    {
        static void Main()
        {
            var scenariosConfiguration = Configure();

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            IScheduler scheduler = schedulerFactory.GetScheduler();
            scheduler.ListenerManager.AddJobListener(new JobTrigerringListener(scenariosConfiguration));

            foreach (var config in scenariosConfiguration)
            {
                var groupName = "metrics";
                var job = JobBuilder.Create<FakeJob>()
                    .WithIdentity(config.Key, groupName)
                    .Build();

                var triggeringTime = DateTime.UtcNow.AddSeconds(5);
                var triggerDefinition = TriggerBuilder.Create()
                    .StartAt(triggeringTime)
                    .WithSimpleSchedule();

                scheduler.ScheduleJob(job, triggerDefinition
                    .WithIdentity(config.Key, groupName)
                    .Build());
            }

            scheduler.Start();

            Thread.Sleep(60*1000);

            scheduler.Shutdown();

            Console.ReadKey();
        }

        private static Dictionary<string, int> Configure()
        {
            var scenariosConfiguration = new Dictionary<string, int>
                                             {
                                                { "IdolScenario", 5 },
                                                { "EdcScenario", 5 }
                                             };


            return scenariosConfiguration;
        }
    }
}
