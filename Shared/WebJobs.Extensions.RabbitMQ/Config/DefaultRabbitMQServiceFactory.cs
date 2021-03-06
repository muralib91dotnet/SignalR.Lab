﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Azure.WebJobs.Extensions.RabbitMQ
{
    internal class DefaultRabbitMQServiceFactory : IRabbitMQServiceFactory
    {
        public IRabbitMQService CreateService(string connectionString, string hostName, string queueName, string userName, string password, int port, string deadLetterExchangeName)
        {
            return new RabbitMQService(connectionString, hostName, queueName, userName, password, port, deadLetterExchangeName);
        }

        public IRabbitMQService CreateService(string connectionString, string hostName, QueueSetting queueSetting, string userName, string password, int port, ExchangeSetting deadLetterExchangeSetting)
        {
            return new RabbitMQService(connectionString, hostName, queueSetting, userName, password, port, deadLetterExchangeSetting);
        }

        public IRabbitMQService CreateService(string connectionString, string hostName, string userName, string password, int port)
        {
            return new RabbitMQService(connectionString, hostName, userName, password, port);
        }
    }
}
