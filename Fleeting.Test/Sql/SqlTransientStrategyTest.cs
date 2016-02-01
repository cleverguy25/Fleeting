// <copyright file="SqlTransientStrategyTest.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Test.Sql
{
    using System;
    using Fleeting.Sql;
    using Xunit;

    public class SqlTransientStrategyTest
    {
        [Theory]
        [InlineData(10929)]
        [InlineData(11001)]
        [InlineData(40501)]
        public void TransientSqlExceptions(int errorNumber)
        {
            // Arrange
            var sqlException = SqlExceptionCreator.CreateSqlException(errorNumber);

            // Act
            var result = SqlTransientStrategy.IsTransient(sqlException);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(131)]
        [InlineData(500)]
        [InlineData(10712)]
        public void NonTransientSqlExceptions(int errorNumber)
        {
            // Arrange
            var sqlException = SqlExceptionCreator.CreateSqlException(errorNumber);

            // Act
            var result = SqlTransientStrategy.IsTransient(sqlException);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TimeoutIsTransient()
        {
            // Arrange
            var exception = new TimeoutException();

            // Act
            var result = SqlTransientStrategy.IsTransient(exception);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void InvalidOperationIsNotTransient()
        {
            // Arrange
            var exception = new InvalidOperationException();

            // Act
            var result = SqlTransientStrategy.IsTransient(exception);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void NullExcecptionIsNotTransient()
        {
            // Arrange
            // Act
            var result = SqlTransientStrategy.IsTransient(null);

            // Assert
            Assert.False(result);
        }
    }
}