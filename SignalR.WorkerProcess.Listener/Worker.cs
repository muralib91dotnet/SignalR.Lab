using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using NServiceBus;
using Microsoft.Extensions.Configuration;
using SignalR.WorkerProcess.Listener.Helper;
using SignalR.WorkerProcess.Listener.Model;
using SignalR.WorkerProcess.Message.Model;
using Newtonsoft.Json;

namespace SignalR.WorkerProcess.Listener
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly ValueHubClient _valueHubClient;

        public Worker(ILogger<Worker> logger, IConfiguration config, ValueHubClient valueHubClient)
        {
            _logger = logger;
            _config = config;
            _valueHubClient = valueHubClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var configSection = _config.GetSection("NServiceBusSettings");
            var serviceBusConfig = new ServiceBusConfig();
            configSection.Bind(serviceBusConfig);
            var endpointConfiguration = NServiceBusHelper.GetEndpointConfiguration(configSection);
            endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    //configureComponents.RegisterSingleton(new LoggingHandler(_logger));
                }
            );

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(
                type =>
                {
                    return type.Namespace == "SignalR.WorkerProcess.Message.Model" || type.Namespace == "System" || type.Namespace == "System.Text.Json.JsonElement";
                }
            );
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            await _valueHubClient.StartHubConnection(new Dictionary<string, Action<object>> {
                { 
                    "Add", async (value) => {

                        var exampleMessage = JsonConvert.DeserializeObject<ExampleMessage>(value.ToString());
                        //Push to Nservicebus-RabbitMQ Queue
                        //await endpointInstance.Send(serviceBusConfig.ToEndpointQueue,exampleMessage);
                        await endpointInstance.Send(serviceBusConfig.ToEndpointQueue,exampleMessage.Value);
                    } 
                }
            });

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker stopped at: {DateTime.Now}");

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");

            base.Dispose();
        }
    }
}
