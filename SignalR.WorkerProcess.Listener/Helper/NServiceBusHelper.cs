using System;
using System.Collections.Generic;
using System.Text;
using NServiceBus;
using NServiceBus.Transport;
using Microsoft.Extensions.Configuration;
using SignalR.WorkerProcess.Listener.Model;

namespace SignalR.WorkerProcess.Listener.Helper
{
    //public class NServiceBusHelper<TransportType> where TransportType: TransportDefinition
    public class NServiceBusHelper
    {
        public static EndpointConfiguration GetEndpointConfiguration(IConfigurationSection configSection)
        {
            var serviceBusConfig = new ServiceBusConfig();
            configSection.Bind(serviceBusConfig);

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration(serviceBusConfig.FromEndpointQueue);
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableInstallers();
            //TODO: Factory Wrapper class for UseTransport<TransportType> if TransportType to be passed & injected from config
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseDirectRoutingTopology();
            transport.ConnectionString(serviceBusConfig.ServiceBusTransportString);


            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("auditqueue");
            var metrics = endpointConfiguration.EnableMetrics();

            metrics.SendMetricDataToServiceControl(
                serviceControlMetricsAddress: "Particular.Monitoring",
                interval: TimeSpan.FromSeconds(2)
            );
            var recoverabilityImmediate = endpointConfiguration.Recoverability();
            recoverabilityImmediate.Immediate(
                immediate =>
                {
                    immediate.NumberOfRetries(serviceBusConfig.ImmediateNumberOfRetries);
                });
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(serviceBusConfig.DelayedNumberOfRetries);
                    delayed.TimeIncrease(TimeSpan.FromSeconds(serviceBusConfig.DelayTimeSpanInSeconds));
                });
            return endpointConfiguration;
        }
    }
}
