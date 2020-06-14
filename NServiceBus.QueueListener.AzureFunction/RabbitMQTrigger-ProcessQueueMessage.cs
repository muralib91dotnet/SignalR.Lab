using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NServiceBus.QueueListener.AzureFunction
{
    public static class RabbitMQTrigger_ProcessQueueMessage
    {

        //static RabbitMQTrigger_ProcessQueueMessage() { }

        //public static void TimerTrigger_ConnectionString_StringOutput(
        //    [TimerTrigger("00:03", RunOnStartup = true)] TimerInfo timer,
        //    [RabbitMQ(ConnectionStringSetting = "RabbitMQConnection")] IModel client,
        //    ILogger logger)
        //{
        //    QueueDeclareOk queue = client.QueueDeclare("example.server", true, false, false, null);
        //    logger.LogInformation("Opening connection and creating/connecting to existing queue!");
        //}

        [FunctionName("RabbitMqListener")]
        public static void Run(
        [RabbitMQTrigger("example.server", HostName = "localhost", UserNameSetting = "guest", PasswordSetting = "guest", Port = 5672, _QueueSetting = "{IsDurable:true}")] string inputMessage,
        //[RabbitMQTrigger("localhost", "guest", "guest", 5672, "example.server", "{IsDurable:true}")] string inputMessage,
        [RabbitMQ(ConnectionStringSetting = "RabbitMQConnection", QueueName = "downstream")] out string outputMessage,
        ILogger logger)
        {
            outputMessage = inputMessage;
            Console.WriteLine("Message posted to RabbitMq");
            logger.LogInformation($"RabittMQ output binding function sent message: {inputMessage}");
        }
    }
}
