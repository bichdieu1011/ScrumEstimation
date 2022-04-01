using Quartz;

namespace ScrumEstimationServerApp.Jobs
{
    public class JobManager
    {
        //private static readonly ILogger logger = LogManager.GetCurrentClassLogger(typeof(JobManager));

        public static void BuildQuartzConfig(ref IServiceCollectionQuartzConfigurator taskScheduler)
        {
            taskScheduler.AddJob<SyncJob>(opts => opts.WithIdentity(nameof(SyncJob)));
            taskScheduler.AddTrigger(opts => opts
                    .ForJob(nameof(SyncJob)) // link to the HelloWorldJob
                    .WithIdentity($"{nameof(SyncJob)}-trigger") // give the trigger a unique name
                    .WithSimpleSchedule(s => s.WithIntervalInMinutes(1).RepeatForever()));
        }
    }
}
