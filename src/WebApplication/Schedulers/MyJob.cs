using FluentScheduler;

namespace DanLiris.Admin.Web.Schedulers
{
    public class SampleJob : IJob
    {
        private readonly object _lock = new object();

        private bool _shuttingDown;

        public SampleJob()
        {
            // Register this job with the hosting environment.
            // Allows for a more graceful stop of the job, in the case of IIS shutting down.
            //HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            try
            {
                lock (_lock)
                {
                    if (_shuttingDown)
                        return;

                    // Do work, son!
                }
            }
            finally
            {
                // Always unregister the job when done.
                //HostingEnvironment.UnregisterObject(this);
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {
                _shuttingDown = true;
            }

            //HostingEnvironment.UnregisterObject(this);
        }
    }
}