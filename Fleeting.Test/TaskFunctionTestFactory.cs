// <copyright file="TaskFunctionTestFactory.cs" company="cleve.littlefield Open Source">
//   Copyright (c) cleve.littlefield Open Source. All rights reserved. See License.txt in the project root for license information.
// </copyright>

namespace Fleeting.Test
{
    using System;
    using System.Threading.Tasks;

    public class TaskFunctionTestFactory
    {
        public static Func<Task<bool>> GetTaskFunctionTResultWithRetry(int maxRetryCount = 2)
        {
            var retryCount = 0;
            Func<Task<bool>> taskFunction = () =>
            {
                retryCount++;
                if (retryCount <= maxRetryCount)
                {
                    throw new TimeoutException("Timeout");
                }

                return Task.FromResult(true);
            };

            return taskFunction;
        }

        public static Func<Task> GetTaskFunctionWithRetry(int maxRetryCount = 2)
        {
            var retryCount = 0;
            Func<Task> taskFunction = () =>
            {
                retryCount++;
                if (retryCount <= maxRetryCount)
                {
                    throw new TimeoutException("Timeout");
                }

                return Task.Delay(0);
            };

            return taskFunction;
        }
    }
}