using FluentScheduler;

namespace DanLiris.Admin.Web.Schedulers
{
    public class DefaultScheduleRegistry : Registry
    {
        public DefaultScheduleRegistry()
        {
            // Schedule an IJob to run at an interval
            Schedule<SampleJob>().ToRunNow().AndEvery(30).Seconds();

            // Schedule an IJob to run once, delayed by a specific time interval
            //Schedule<MyJob>().ToRunOnceIn(5).Seconds();

            // Schedule a simple job to run at a specific time
            //Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);

            // Schedule a more complex action to run immediately and on an monthly interval
            //Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

            // Schedule a job using a factory method and pass parameters to the constructor.
            //Schedule(() => new MyComplexJob("Foo", DateTime.Now)).ToRunNow().AndEvery(2).Seconds();

            // Schedule multiple jobs to be run in a single schedule
            //Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
        }
    }
}