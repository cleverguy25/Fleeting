// <copyright file="SqlTransientStrategy.cs" company="Palador Open Source">
//   Copyright (c) Palador Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class SqlTransientStrategy
    {
        private const int LoginFailed = 4060;

        private const int Throttled = 40501;

        private const int LimitReached = 10928;

        private const int TooBusy = 10929;

        private const int ConnectionAborted = 10053;

        private const int TcpConnectionForciblyClosed = 10054;

        private const int ServerNotFoundOrInaccessible = 10060;

        private const int ServiceEncounteredError = 40197;

        private const int ServiceEncounteredError2 = 40540;

        private const int DatabaseNotAvailable = 40613;

        private const int ServiceEncounteredError3 = 40143;

        private const int UnableToEstablishConnection = 233;

        private const int LoginServerNoLongerAvailable = 64;

        private const int NetworkConnectivity = 11001;

        private const int TimeoutExpired = -2;

        private const int EncryptionNotSupported = 20;

        private static readonly Dictionary<int, bool> TransientErrors = new Dictionary<int, bool>
                                                                            {
                                                                                {
                                                                                    NetworkConnectivity, true
                                                                                },
                                                                                {
                                                                                    Throttled, true
                                                                                },
                                                                                {
                                                                                    LoginFailed, true
                                                                                },
                                                                                {
                                                                                    LimitReached, true
                                                                                },
                                                                                {
                                                                                    TooBusy, true
                                                                                },
                                                                                {
                                                                                    ConnectionAborted, true
                                                                                },
                                                                                {
                                                                                    TcpConnectionForciblyClosed, true
                                                                                },
                                                                                {
                                                                                    ServerNotFoundOrInaccessible, true
                                                                                },
                                                                                {
                                                                                    ServiceEncounteredError, true
                                                                                },
                                                                                {
                                                                                    ServiceEncounteredError2, true
                                                                                },
                                                                                {
                                                                                    DatabaseNotAvailable, true
                                                                                },
                                                                                {
                                                                                    ServiceEncounteredError3, true
                                                                                },
                                                                                {
                                                                                    UnableToEstablishConnection, true
                                                                                },
                                                                                {
                                                                                    LoginServerNoLongerAvailable, true
                                                                                },
                                                                                {
                                                                                    TimeoutExpired, true
                                                                                },
                                                                                {
                                                                                    EncryptionNotSupported, true
                                                                                }
                                                                            };

        public static bool IsTransient(Exception exception)
        {
            if (exception == null)
            {
                return false;
            }

            if (exception is TimeoutException)
            {
                return true;
            }

            var sqlException = exception as SqlException;
            if (sqlException == null)
            {
                return false;
            }

            return sqlException.Errors.Cast<SqlError>().Any(error => TransientErrors.ContainsKey(error.Number));
        }
    }
}