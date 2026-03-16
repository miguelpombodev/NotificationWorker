using Polly;
using Polly.Retry;

namespace NotificationWorker.Infrastructure;

public static class RetryPolicies
{
    public static AsyncRetryPolicy EmailRetry =>
        Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                3,
                retry => TimeSpan.FromSeconds(2));
}