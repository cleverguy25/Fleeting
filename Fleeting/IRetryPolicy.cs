// <copyright file="IRetryPolicy.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting
{
    using System;
    using System.Threading.Tasks;

    public interface IRetryPolicy
    {
        event EventHandler<RetryEventArgs> Retry;

        bool ShouldRetry(Exception exception, int retryCount);

        TimeSpan GetRetryInterval(int retryCount);

        void RaiseRetryEvent(int retryCount, Exception exception, TimeSpan delay);

        Task<TResult> ExecuteAsyncWithRetry<TResult>(Func<Task<TResult>> taskFunction);

        Task ExecuteAsyncWithRetry(Func<Task> taskFunction);
    }
}