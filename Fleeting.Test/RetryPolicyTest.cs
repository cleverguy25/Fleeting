// <copyright file="RetryPolicyTest.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Test
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class RetryPolicyTest
    {
        [Fact]
        public async void ExecuteAsyncTResultSuccess()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            Func<Task<bool>> taskFunction = () => Task.FromResult(true);

            // Act
            var result = await retryPolicy.ExecuteAsyncWithRetry(taskFunction);

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public async void ExecuteAsyncTResultWithRetry()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            var taskFunction = TaskFunctionTestFactory.GetTaskFunctionTResultWithRetry();

            // Act
            var result = await retryPolicy.ExecuteAsyncWithRetry(taskFunction);

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        public async void ExecuteAsyncTResultWithRetryFiresEvent()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            var taskFunction = TaskFunctionTestFactory.GetTaskFunctionTResultWithRetry();

            var retryCount = 0;
            retryPolicy.Retry += (sender, args) =>
            {
                retryCount++;
                Assert.Equal(typeof(TimeoutException), args.Exception.GetType());
                Assert.Equal(retryCount, args.RetryCount);
                Assert.Equal(100, args.Delay.TotalMilliseconds);
            };

            // Act
            await retryPolicy.ExecuteAsyncWithRetry(taskFunction);

            // Assert
            Assert.Equal(2, retryCount);
        }

        [Fact]
        public async void ExecuteAsyncTResultWithError()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            Func<Task<bool>> taskFunction = () =>
            {
                throw new InvalidOperationException("Error");
            };

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => retryPolicy.ExecuteAsyncWithRetry(taskFunction));
        }

        [Fact]
        public async void ExecuteAsyncTResultWithMaxRetries()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            var taskFunction = TaskFunctionTestFactory.GetTaskFunctionTResultWithRetry(5);

            // Act
            await Assert.ThrowsAsync<TimeoutException>(() => retryPolicy.ExecuteAsyncWithRetry(taskFunction));
        }

        [Fact]
        public async void ExecuteAsyncSuccess()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            Func<Task> taskFunction = () => Task.Delay(0);

            // Act
            await retryPolicy.ExecuteAsyncWithRetry(taskFunction);
        }

        [Fact]
        public async void ExecuteAsyncWithRetry()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            var taskFunction = TaskFunctionTestFactory.GetTaskFunctionWithRetry();

            // Act
            await retryPolicy.ExecuteAsyncWithRetry(taskFunction);
        }

        [Fact]
        public async void ExecuteAsyncWithRetryFiresEvent()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            var taskFunction = TaskFunctionTestFactory.GetTaskFunctionWithRetry();

            var retryCount = 0;
            retryPolicy.Retry += (sender, args) =>
            {
                retryCount++;
                Assert.Equal(typeof(TimeoutException), args.Exception.GetType());
                Assert.Equal(retryCount, args.RetryCount);
            };

            // Act
            await retryPolicy.ExecuteAsyncWithRetry(taskFunction);

            // Assert
            Assert.Equal(2, retryCount);
        }

        [Fact]
        public async void ExecuteAsyncWithMaxRetries()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            var taskFunction = TaskFunctionTestFactory.GetTaskFunctionWithRetry(5);

            // Act
            await Assert.ThrowsAsync<TimeoutException>(() => retryPolicy.ExecuteAsyncWithRetry(taskFunction));
        }

        [Fact]
        public async void ExecuteAsyncWithError()
        {
            // Arrange
            var retryPolicy = GetRetryPolicy();
            Func<Task> taskFunction = () =>
            {
                throw new InvalidOperationException("Error");
            };

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => retryPolicy.ExecuteAsyncWithRetry(taskFunction));
        }

        private static IRetryPolicy GetRetryPolicy(int retryCount = 3)
        {
            return new RetryPolicy(exception => exception is TimeoutException, retryCount, RetryIntervalFactory.GetFixedInterval(100));
        }
    }
}