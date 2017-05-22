using System;
using System.Collections.Generic;
using Quartz;

namespace QuartzPoC
{
    public class JobTrigerringListener : IJobListener
    {
        private readonly Dictionary<string, int> scenariosTrigerringIntervals;
        public JobTrigerringListener(Dictionary<string, int> scenariosConfiguration)
        {
            scenariosTrigerringIntervals = scenariosConfiguration;
        }
        public void JobToBeExecuted(IJobExecutionContext context)
        {
            //// Before job start -> log it!
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            //// After job execution -> log job name and execution time!

            var triggeringTime = DateTime.UtcNow.AddSeconds(scenariosTrigerringIntervals[context.JobDetail.Key.Name]);
            var trigger = context.Trigger.GetTriggerBuilder().StartAt(triggeringTime).Build();

            var job = context.JobDetail.GetJobBuilder().Build();
            context.Scheduler.DeleteJob(context.JobDetail.Key);
            context.Scheduler.ScheduleJob(job, trigger);
        }

        public string Name => "JobTrigerringListener";
    }
}