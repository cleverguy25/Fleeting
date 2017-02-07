// <copyright file="RetryPolicy.cs" company="cleve.littlefield Open Source">
//   Copyright (c) cleve.littlefield Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting
{
    using System;
    using System.Threading.Tasks;

    public class RetryPolicy : IRetryPolicy
    {
        private readonly Func<int, TimeSpan> getRetryInterval;

        private readonly Func<Exception, bool> isTransient;

        private readonly int maxRetryCount;

        public RetryPolicy(Func<Exception, bool> isTransient, int maxRetryCount, Func<int, TimeSpan> getRetryInterval)
        {
            this.isTransient = isTransient;
            this.maxRetryCount = maxRetryCount;
            this.getRetryInterval = getRetryInterval;
        }

        public event EventHandler<RetryEventArgs> Retry;

        public Task<TResult> ExecuteAsyncWithRetry<TResult>(Func<Task<TResult>> taskFunction)
        {
            var context = new AsyncRetryContext<TResult>(taskFunction, this);
            return context.ExecuteAsyncWithRetry();
        }

        public Task ExecuteAsyncWithRetry(Func<Task> taskFunction)
        {
            var context = new AsyncRetryContext<bool>(
                async () =>
                {
                    await taskFunction();
                    return true;
                }, this);

            return context.ExecuteAsyncWithRetry();
        }

        public virtual bool ShouldRetry(Exception exception, int retryCount)
        {
            return this.isTransient(exception) && retryCount < this.maxRetryCount;
        }

        public virtual TimeSpan GetRetryInterval(int retryCount)
        {
            return this.getRetryInterval(retryCount);
        }

        public virtual void RaiseRetryEvent(int retryCount, Exception exception, TimeSpan delay)
        {
            this.Retry?.Invoke(this, new RetryEventArgs(retryCount, delay, exception));
        }
    }
}