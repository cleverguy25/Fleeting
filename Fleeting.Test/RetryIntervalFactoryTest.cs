// <copyright file="RetryIntervalFactoryTest.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Test
{
    using Xunit;

    public class RetryIntervalFactoryTest
    {
        [Theory]
        [InlineData(1, 200, 200)]
        [InlineData(2, 100, 100)]
        public void FixedInterval(int retryCount, int interval, int expectedResult)
        {
            // Arrange
            var getRetryInterval = RetryIntervalFactory.GetFixedInterval(interval);

            // Act
            var result = getRetryInterval(retryCount);

            // Assert
            Assert.Equal(expectedResult, result.TotalMilliseconds);
        }

        [Theory]
        [InlineData(1, 200, 200)]
        [InlineData(2, 250, 500)]
        public void LinearInterval(int retryCount, int interval, int expectedResult)
        {
            // Arrange
            var getRetryInterval = RetryIntervalFactory.GetLinearInterval(interval);

            // Act
            var result = getRetryInterval(retryCount);

            // Assert
            Assert.Equal(expectedResult, result.TotalMilliseconds);
        }

        [Theory]
        [InlineData(1, 100, 100)]
        [InlineData(2, 200, 400)]
        [InlineData(3, 300, 1200)]
        [InlineData(3, 100, 400)]
        public void ExponentialInterval(int retryCount, int interval, int expectedResult)
        {
            // Arrange
            var getRetryInterval = RetryIntervalFactory.GetExponentialInterval(interval);

            // Act
            var result = getRetryInterval(retryCount);

            // Assert
            Assert.Equal(expectedResult, result.TotalMilliseconds);
        }
    }
}