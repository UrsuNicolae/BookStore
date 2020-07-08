using BookStore.BusinessLogic.ServiceModels;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.HealthCheck
{
    public class BackgroundServiceCheck : IHealthCheck
    {
        private volatile int _starttupTaskCompleted = 0;

        public int StartupTaskCompleted
        {
            get => _starttupTaskCompleted;
            set => _starttupTaskCompleted = value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (StartupTaskCompleted == 1)
            {
                return Task.FromResult(HealthCheckResult.Degraded("Startup task completed with delay"));
            }
            if(StartupTaskCompleted == 2)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Startup task completed"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Startup tasks not complete"));
        }
    }
}
