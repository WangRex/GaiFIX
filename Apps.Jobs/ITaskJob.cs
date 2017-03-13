using Apps.Models.JOB;
namespace Apps.Jobs
{
    interface ITaskJob
    {
        string RunJobBefore(Job jobModel);
        string CloseJob(Job jobModel);
        string RunJob(ref Quartz.JobDataMap dataMap, string jobName, string id, string taskName);
    }
}
