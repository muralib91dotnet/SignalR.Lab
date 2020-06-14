// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Extensions.RabbitMQ;
using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs
{
    [Binding]
    public sealed class RabbitMQTriggerAttribute : Attribute
    {

        public RabbitMQTriggerAttribute(string queueName)
        {
            QueueName = queueName;

            //if (!string.IsNullOrEmpty(_QueueSetting))
            //{
            //    QueueSetting = JsonConvert.DeserializeObject<QueueSetting>(_QueueSetting);
            //    QueueSetting.Name = queueName;
            //}
        }

        public RabbitMQTriggerAttribute(string hostName, string userNameSetting, string passwordSetting, int port, string queueName)
        {
            HostName = hostName;
            UserNameSetting = userNameSetting;
            PasswordSetting = passwordSetting;
            Port = port;
            //QueueSetting = JsonConvert.DeserializeObject<QueueSetting>(queueName);
            //QueueSetting.Name = queueName;
        }

        //public RabbitMQTriggerAttribute(string hostName, string userNameSetting, string passwordSetting, int port, string queueName, [Description("should be Json string")] string queueSetting = null, [Description("should be Json string")] string exchangeSetting = null)
        //{
        //    try
        //    {
        //        HostName = hostName;
        //        UserNameSetting = userNameSetting;
        //        PasswordSetting = passwordSetting;
        //        Port = port;

        //        QueueSetting = JsonConvert.DeserializeObject<QueueSetting>(queueSetting);
        //        QueueSetting.Name = queueName;

        //        ExchangeSetting = JsonConvert.DeserializeObject<ExchangeSetting>(exchangeSetting);
        //    }
        //    catch (JsonSerializationException ex)
        //    {
        //        throw ex;
        //    }
        //}

        [ConnectionString]
        public string ConnectionStringSetting { get; set; }

        public string HostName { get; set; }

        public string QueueName { get; }

        internal QueueSetting QueueSetting { get; set; }

        public string _QueueSetting 
        {
            get
            {
                return JsonConvert.SerializeObject(QueueSetting);
            }

            set
            {
                if (value != null)
                {
                    QueueSetting = JsonConvert.DeserializeObject<QueueSetting>(value);
                    QueueSetting.Name = QueueName;
                }
            }
        }

        public ExchangeSetting ExchangeSetting { get; }

        [AppSetting]
        public string UserNameSetting { get; set; }

        [AppSetting]
        public string PasswordSetting { get; set; }

        public int Port { get; set; }

        public string DeadLetterExchangeName { get; set; }
    }
}
