// <copyright file="SqlRetryPolicyFactory.cs" company="cleve.littlefield Open Source">
//   Copyright (c) cleve.littlefield Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Sql
{
    using System;

    public class SqlRetryPolicyFactory
    {
        private static readonly Lazy<IRetryPolicy> DefaultInstance = new Lazy<IRetryPolicy>(() => CreateSqlRetryPolicy());

        public static IRetryPolicy DefaultSqlRetryPolicy => DefaultInstance.Value;

        public static IRetryPolicy CreateSqlRetryPolicy(int maxRetryCount = 3, int linearIntervalMilliseconds = 500)
        {
            var retryPolicy = new RetryPolicy(
                SqlTransientStrategy.IsTransient,
                maxRetryCount,
                RetryIntervalFactory.GetLinearInterval(linearIntervalMilliseconds));

            retryPolicy.Retry += (sender, args) => SqlRetryEventSource.Current.LogTransientException(args);

            return retryPolicy;
        }
    }
}