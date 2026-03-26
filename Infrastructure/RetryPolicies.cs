using Polly;
using Polly.Retry;

namespace NotificationWorker.Infrastructure;

public static class RetryPolicies
{
    private static readonly HashSet<Type> NonRetriableExceptions =
    [
        typeof(DirectoryNotFoundException),
        typeof(FileNotFoundException)
    ];
    
    public static ResiliencePipeline EmailRetry(ILogger logger) =>
        new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts    = 3,
                Delay               = TimeSpan.FromSeconds(2),
                BackoffType         = DelayBackoffType.Constant,
                ShouldHandle        = args =>
                {
                    var ex = args.Outcome.Exception;

                    if (ex is null) return ValueTask.FromResult(false);
                    
                    if (NonRetriableExceptions.Contains(ex.GetType()))
                        return ValueTask.FromResult(false);
                    
                    var retriable = ex is HttpRequestException
                        or TimeoutException
                        or IOException;

                    return ValueTask.FromResult(retriable);
                },
                OnRetry = args =>
                {
                    logger.LogWarning(
                        args.Outcome.Exception,
                        "Retry {Attempt}/3 after {Delay}s — {ExceptionType}: {Message}",
                        args.AttemptNumber + 1,
                        args.RetryDelay.TotalSeconds,
                        args.Outcome.Exception?.GetType().Name,
                        args.Outcome.Exception?.Message);

                    return ValueTask.CompletedTask;
                }
            })
            .Build();
}