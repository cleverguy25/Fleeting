// <copyright file="AsyncRetryContext.cs" company="cleve.littlefield Open Source">
//   Copyright (c) cleve.littlefield Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting
{
    using System;
    using System.Threading.Tasks;

    public class AsyncRetryContext<TResult>
    {
        private readonly IRetryPolicy retryPolicy;

        private readonly Func<Task<TResult>> taskFunction;

        private int retryCount;

        public AsyncRetryContext(Func<Task<TResult>> taskFunction, IRetryPolicy retryPolicy)
        {
            this.taskFunction = taskFunction;
            this.retryPolicy = retryPolicy;
        }

        public Task<TResult> ExecuteAsyncWithRetry()
        {
            Task<TResult> task;
            try
            {
                task = this.taskFunction();
            }
            catch (Exception exception)
            {
                var exceptionTask = new TaskCompletionSource<TResult>();
                exceptionTask.SetException(exception);
                return this.RetryContinue(exceptionTask.Task);
            }

            return task.ContinueWith(this.RetryContinue).Unwrap();
        }

        private Task<TResult> RetryContinue(Task<TResult> currentTask)
        {
            if (currentTask.IsFaulted == false)
            {
                return currentTask;
            }

            var exception = currentTask.Exception?.InnerException;
            this.retryCount++;
            if (this.retryPolicy.ShouldRetry(exception, this.retryCount) == false)
            {
                return currentTask;
            }

            var delay = this.retryPolicy.GetRetryInterval(this.retryCount);
            this.retryPolicy.RaiseRetryEvent(this.retryCount, exception, delay);

            return Task.Delay(delay)
                       .ContinueWith(_ => this.ExecuteAsyncWithRetry())
                       .Unwrap();
        }
    }
}