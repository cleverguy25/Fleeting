// <copyright file="SqlExceptionCreator.cs" company="cleve.littlefield Open Source">
//   Copyright (c) cleve.littlefield Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Test.Sql
{
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;

    internal class SqlExceptionCreator
    {
        internal static SqlException CreateSqlException(int number = 1)
        {
            var collection = Construct<SqlErrorCollection>();
            var error = Construct<SqlError>(number, (byte)2, (byte)3, "server name", "error message", "proc", 100, null);

            typeof(SqlErrorCollection).GetTypeInfo()
                .GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new object[] { error });

            var methods = typeof(SqlException).GetTypeInfo()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

            var method = methods.First(item => item.Name == "CreateException");

            return method.Invoke(
                    null,
                    new object[] { collection, "7.0.0" }) as SqlException;
        }

        private static T Construct<T>(params object[] p)
        {
            var constructors = typeof(T).GetTypeInfo().GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)constructors.First(ctor => ctor.GetParameters().Length == p.Length).Invoke(p);
        }
    }
}