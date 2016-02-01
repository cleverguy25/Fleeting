// <copyright file="RetryIntervalFactory.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting
{
    using System;

    public static class RetryIntervalFactory
    {
        public static Func<int, TimeSpan> GetFixedInterval(int intervalMilliseconds)
        {
            return GetFixedInterval(TimeSpan.FromMilliseconds(intervalMilliseconds));
        }

        public static Func<int, TimeSpan> GetFixedInterval(TimeSpan interval)
        {
            return retryCount => interval;
        }

        public static Func<int, TimeSpan> GetLinearInterval(int intervalMilliseconds)
        {
            return GetLinearInterval(TimeSpan.FromMilliseconds(intervalMilliseconds));
        }

        public static Func<int, TimeSpan> GetLinearInterval(TimeSpan interval)
        {
            return retryCount => TimeSpan.FromMilliseconds(interval.TotalMilliseconds * retryCount);
        }

        public static Func<int, TimeSpan> GetExponentialInterval(int deltaMilliseconds)
        {
            return GetExponentialInterval(TimeSpan.FromMilliseconds(deltaMilliseconds));
        }

        public static Func<int, TimeSpan> GetExponentialInterval(TimeSpan delta)
        {
            return retryCount =>
            {
                var count = retryCount - 1;
                var intervalMilliseconds = Math.Pow(2, count) * delta.TotalMilliseconds;
                return TimeSpan.FromMilliseconds(intervalMilliseconds);
            };
        }
    }
}