// <copyright file="RetryEventArgs.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting
{
    using System;

    public class RetryEventArgs : EventArgs
    {
        public RetryEventArgs(int currentRetryCount, TimeSpan delay, Exception exception)
        {
            this.RetryCount = currentRetryCount;
            this.Delay = delay;
            this.Exception = exception;
        }

        public int RetryCount { get; private set; }

        public TimeSpan Delay { get; private set; }

        public Exception Exception { get; private set; }
    }
}