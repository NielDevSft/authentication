// The MIT License (MIT)
// 
// Copyright (c) 2015-2024 Rasmus Mikkelsen
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using JWTAuthentication.Domain.Core.Extensions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace JWTAuthentication.Domain.Core.Core
{
    public class TransientFaultHandler<TRetryStrategy> : ITransientFaultHandler<TRetryStrategy>
        where TRetryStrategy : IRetryStrategy
    {
        private readonly ILogger<TransientFaultHandler<TRetryStrategy>> _logger;
        private readonly TRetryStrategy _retryStrategy;

        public TransientFaultHandler(
            ILogger<TransientFaultHandler<TRetryStrategy>> logger,
            TRetryStrategy retryStrategy)
        {
            _logger = logger;
            _retryStrategy = retryStrategy;
        }

        public void ConfigureRetryStrategy(Action<TRetryStrategy> configureStrategy)
        {
            if (configureStrategy == null)
            {
                throw new ArgumentNullException(nameof(configureStrategy));
            }

            configureStrategy(_retryStrategy);
        }

        public Task TryAsync(
            Func<CancellationToken, Task> action,
            Label label,
            CancellationToken cancellationToken)
        {
            return TryAsync<object>(
                async c =>
                    {
                        await action(c).ConfigureAwait(false);
                        return null;
                    },
                label,
                cancellationToken);
        }

        public async Task<T> TryAsync<T>(Func<CancellationToken, Task<T>> action, Label label, CancellationToken cancellationToken)
        {
            if (_retryStrategy == null)
            {
                throw new InvalidOperationException("You need to configure the retry strategy using the Use(...) method");
            }

            var stopwatch = Stopwatch.StartNew();
            var currentRetryCount = 0;

            while (true)
            {
                Exception currentException;
                Retry retry;
                try
                {
                    var result = await action(cancellationToken).ConfigureAwait(false);
                    _logger.LogTrace(
                        "Finished execution of {Label} after {RetryCount} retries and {Seconds} seconds",
                        label,
                        currentRetryCount,
                        stopwatch.Elapsed.TotalSeconds);
                    return result;
                }
                catch (Exception exception)
                {
                    currentException = exception;
                    var currentTime = stopwatch.Elapsed;
                    retry = _retryStrategy.ShouldThisBeRetried(currentException, currentTime, currentRetryCount);
                    if (!retry.ShouldBeRetried)
                    {
                        throw;
                    }
                }

                currentRetryCount++;
                if (retry.RetryAfter != TimeSpan.Zero)
                {
                    _logger.LogTrace(
                        "Exception {ExceptionType} with message {ExceptionMessage} is transient, retrying action {Label} after {Seconds} seconds for retry count {RetryCount}",
                        currentException.GetType().PrettyPrint(),
                        currentException.Message,
                        label,
                        retry.RetryAfter.TotalSeconds,
                        currentRetryCount);
                    await Task.Delay(retry.RetryAfter, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    _logger.LogTrace(
                        "Exception {ExceptionType} with message {ExceptionMessage} is transient, retrying action {Label} NOW for retry count {RetryCount}",
                        currentException.GetType().PrettyPrint(),
                        currentException.Message,
                        label,
                        currentRetryCount);
                }
            }
        }
    }
}