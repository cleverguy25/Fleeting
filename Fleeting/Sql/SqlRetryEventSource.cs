// <copyright file="SqlRetryEventSource.cs" company="cleve.littlefield Open Source">
//   Copyright (c) cleve.littlefield Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Sql
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics.Tracing;

    [EventSource(Name = "RetryAsync-Sql")]
    public class SqlRetryEventSource : EventSource
    {
        private const int SqlRetrySqlExceptionEventId = 1;

        private const int SqlRetryEventId = 2;

        private static readonly Lazy<SqlRetryEventSource> CurrentInstance =
            new Lazy<SqlRetryEventSource>(() => new SqlRetryEventSource());

        public static SqlRetryEventSource Current => CurrentInstance.Value;

        [NonEvent]
        public void LogTransientException(RetryEventArgs args)
        {
            var exception = args.Exception;
            var sqlException = exception as SqlException;
            if (sqlException == null)
            {
                this.SqlRetry(exception.Message, exception.StackTrace, args.RetryCount, args.Delay.TotalMilliseconds);
                return;
            }

            foreach (SqlError sqlError in sqlException.Errors)
            {
                this.SqlRetrySqlException(
                    exception.Message,
                    exception.StackTrace,
                    sqlError.Number,
                    args.RetryCount,
                    args.Delay.TotalMilliseconds);
            }
        }

        [Event(SqlRetrySqlExceptionEventId, Level = EventLevel.Warning, Message = "Sql Retry Sql Exception")]
        public void SqlRetrySqlException(
                                         string message,
                                         string stackTrace,
                                         int sqlErrorNumber,
                                         int retryCount,
                                         double delayMilliseconds)
        {
            this.WriteEvent(
                SqlRetrySqlExceptionEventId,
                message,
                stackTrace,
                sqlErrorNumber,
                retryCount,
                delayMilliseconds);
        }

        [Event(SqlRetryEventId, Level = EventLevel.Warning, Message = "Sql Retry")]
        public void SqlRetry(string message, string stackTrace, int retryCount, double delayMilliseconds)
        {
            this.WriteEvent(SqlRetryEventId, message, stackTrace, retryCount, delayMilliseconds);
        }
    }
}