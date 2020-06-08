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
            var endpointConfiguration = NServiceBusHelper.GetEndpointConfiguration(_config.GetSection("NServiceBusSettings"));
            endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                //configureComponents.RegisterSingleton(new LoggingHandler(_logger));
            });
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            await _valueHubClient.StartHubConnection(new Dictionary<string, Action<object>> {
                { 
                    "Add", async (value) => {
                        //Push to Nservicebus-RabbitMQ Queue
                        await endpointInstance.Send(value);
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
