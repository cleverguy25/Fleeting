// <copyright file="SqlCommandExtensions.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Sql
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    public static class SqlCommandExtensions
    {
        public static Task<int> ExecuteNonQueryAsyncWithRetry(this SqlCommand command)
        {
            return ExecuteNonQueryAsyncWithRetry(command, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task<int> ExecuteNonQueryAsyncWithRetry(SqlCommand command, IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(command.ExecuteNonQueryAsync);
        }

        public static Task<int> ExecuteNonQueryAsyncWithRetry(this SqlCommand command, CancellationToken cancellationToken)
        {
            return ExecuteNonQueryAsyncWithRetry(command, cancellationToken, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task<int> ExecuteNonQueryAsyncWithRetry(
                                                              SqlCommand command,
                                                              CancellationToken cancellationToken,
                                                              IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(() => command.ExecuteNonQueryAsync(cancellationToken));
        }

        public static Task<SqlDataReader> ExecuteReaderAsyncWithRetry(this SqlCommand command)
        {
            return ExecuteReaderAsyncWithRetry(command, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task<SqlDataReader> ExecuteReaderAsyncWithRetry(SqlCommand command, IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(command.ExecuteReaderAsync);
        }

        public static Task<SqlDataReader> ExecuteReaderAsyncWithRetry(
                                                                      this SqlCommand command,
                                                                      CancellationToken cancellationToken)
        {
            return ExecuteReaderAsyncWithRetry(command, cancellationToken, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task<SqlDataReader> ExecuteReaderAsyncWithRetry(
                                                                      SqlCommand command,
                                                                      CancellationToken cancellationToken,
                                                                      IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(() => command.ExecuteReaderAsync(cancellationToken));
        }

        public static Task<SqlDataReader> ExecuteReaderAsyncWithRetry(
                                                                      this SqlCommand command,
                                                                      CommandBehavior behavior)
        {
            return ExecuteReaderAsyncWithRetry(command, behavior, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task<SqlDataReader> ExecuteReaderAsyncWithRetry(
                                                                      SqlCommand command,
                                                                      CommandBehavior behavior,
                                                                      IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(() => command.ExecuteReaderAsync(behavior));
        }

        public static Task<SqlDataReader> ExecuteReaderAsyncWithRetry(
                                                                      this SqlCommand command,
                                                                      CommandBehavior behavior,
                                                                      CancellationToken cancellationToken)
        {
            return ExecuteReaderAsyncWithRetry(command, behavior, cancellationToken, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task<SqlDataReader> ExecuteReaderAsyncWithRetry(
                                                                      SqlCommand command,
                                                                      CommandBehavior behavior,
                                                                      CancellationToken cancellationToken,
                                                                      IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(() => command.ExecuteReaderAsync(behavior, cancellationToken));
        }

        public static Task<object> ExecuteScalarAsyncWithRetry(this SqlCommand command)
        {
            return ExecuteScalarAsyncWithRetry(command, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task<object> ExecuteScalarAsyncWithRetry(
                                                               SqlCommand command,
                                                               IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(command.ExecuteScalarAsync);
        }

        public static Task<object> ExecuteScalarAsyncWithRetry(
                                                               this SqlCommand command,
                                                               CancellationToken cancellationToken)
        {
            return ExecuteScalarAsyncWithRetry(command, cancellationToken, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task<object> ExecuteScalarAsyncWithRetry(
                                                               SqlCommand command,
                                                               CancellationToken cancellationToken,
                                                               IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(() => command.ExecuteScalarAsync(cancellationToken));
        }
    }
}