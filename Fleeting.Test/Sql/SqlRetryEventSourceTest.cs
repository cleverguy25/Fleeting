// <copyright file="SqlRetryEventSourceTest.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Test.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using Fleeting.Sql;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
    using Xunit;

    public class SqlRetryEventSourceTest
    {
        [Fact]
        public void AnalyzeSqlRetryEventSource()
        {
            EventSourceAnalyzer.InspectAll(SqlRetryEventSource.Current);
        }

        [Fact]
        public void LogTransientException()
        {
            // Arrange
            var exception = SqlExceptionCreator.CreateSqlException(-2);
            var eventArgs = new RetryEventArgs(1, TimeSpan.MinValue, exception);
            var listener = new ObservableEventListener();
            var observer = new SqlErrorsObserver();
            listener.EnableEvents(SqlRetryEventSource.Current, EventLevel.LogAlways);
            listener.Subscribe(observer);

            // Act
            SqlRetryEventSource.Current.LogTransientException(eventArgs);

            // Assert
            Assert.Contains(-2, observer.SqlErrors);
        }

        private class SqlErrorsObserver : IObserver<EventEntry>
        {
            public List<int> SqlErrors { get; } = new List<int>();

            public void OnNext(EventEntry value)
            {
                this.SqlErrors.Add((int)value.Payload[2]);
            }

            public void OnCompleted()
            {
            }

            public void OnError(Exception exception)
            {
                Assert.Null(exception);
            }
        }
    }
}