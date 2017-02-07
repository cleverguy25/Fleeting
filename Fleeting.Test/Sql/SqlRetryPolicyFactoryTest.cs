// <copyright file="SqlRetryPolicyFactoryTest.cs" company="cleve.littlefield Open Source">
//   Copyright (c) cleve.littlefield Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Test.Sql
{
    using System;
    using System.Threading.Tasks;
    using Fleeting.Sql;
    using Xunit;

    public class SqlRetryPolicyFactoryTest
    {
        [Fact]
        public async void DefaultSqlRetryPolicy()
        {
            // Arrange
            var retryPolicy = SqlRetryPolicyFactory.DefaultSqlRetryPolicy;

            // Act
            // Assert
            await TestSqlRetryPolicy(retryPolicy, 500);
        }

        [Fact]
        public async void CreateSqlRetryPolicy()
        {
            // Arrange
            var linearIntervalMilliseconds = 100;
            var sqlRetryPolicy = SqlRetryPolicyFactory.CreateSqlRetryPolicy(4, linearIntervalMilliseconds);
            var retryPolicy = sqlRetryPolicy;

            // Act
            // Assert
            await TestSqlRetryPolicy(retryPolicy, linearIntervalMilliseconds);
        }

        [Fact]
        public async void CreateSqlRetryPolicyWithDifferentMaxRetry()
        {
            // Arrange
            var linearIntervalMilliseconds = 300;
            var sqlRetryPolicy = SqlRetryPolicyFactory.CreateSqlRetryPolicy(2, linearIntervalMilliseconds);
            var retryPolicy = sqlRetryPolicy;

            // Act
            // Assert
            await
                Assert.ThrowsAsync<TimeoutException>(() => TestSqlRetryPolicy(retryPolicy, linearIntervalMilliseconds));
        }

        private static async Task TestSqlRetryPolicy(IRetryPolicy retryPolicy, int interval)
        {
            var retryCount = 0;
            retryPolicy.Retry += (sender, args) =>
            {
                retryCount++;

                // Assert
                Assert.Equal(typeof(TimeoutException), args.Exception.GetType());
                Assert.Equal(retryCount, args.RetryCount);
                Assert.Equal(retryCount * interval, args.Delay.TotalMilliseconds);
            };

            var taskFunction = TaskFunctionTestFactory.GetTaskFunctionTResultWithRetry();

            // Act
            await retryPolicy.ExecuteAsyncWithRetry(taskFunction);
        }
    }
}