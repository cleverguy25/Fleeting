// <copyright file="SqlConnectionExtensions.cs" company="cleve.littlefield Open Source">
//   Copyright (c) cleve.littlefield Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Sql
{
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    public static class SqlConnectionExtensions
    {
        public static Task OpenAsyncWithRetry(this SqlConnection connection)
        {
            return OpenAsyncWithRetry(connection, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task OpenAsyncWithRetry(this SqlConnection connection, IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(connection.OpenAsync);
        }

        public static Task OpenAsyncWithRetry(this SqlConnection connection, CancellationToken cancellationToken)
        {
            return OpenAsyncWithRetry(connection, cancellationToken, SqlRetryPolicyFactory.DefaultSqlRetryPolicy);
        }

        public static Task OpenAsyncWithRetry(
                                              this SqlConnection connection,
                                              CancellationToken cancellationToken,
                                              IRetryPolicy retryPolicy)
        {
            return retryPolicy.ExecuteAsyncWithRetry(() => connection.OpenAsync(cancellationToken));
        }
    }
}