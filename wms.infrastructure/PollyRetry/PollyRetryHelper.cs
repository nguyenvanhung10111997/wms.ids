using Polly;

namespace wms.infrastructure.PollyRetry
{
    internal class PollyRetryHelper : IPollyRetryHelper
    {
        public async Task<T> Retry<T>(Func<Task<T>> action)
        {

            bool hasFallback = false;
            Exception ex = null;

            var fallbackPolicy = Policy<T>.Handle<Exception>().FallbackAsync(
                default(T), d =>
                {
                    ex = d.Exception;

                    hasFallback = true;
                    return Task.FromResult(new { });

                });

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (res, timeSpan, context) =>
                    {
                        //log exception
                    });

            var circuitBreaker = Policy.Handle<Exception>().CircuitBreaker(3, TimeSpan.FromSeconds(30));
            var policyResult = await fallbackPolicy.WrapAsync(retryPolicy).ExecuteAndCaptureAsync(action);

            if (hasFallback && ex != null)
                throw ex;

            return policyResult.Result;
        }
    }
}
