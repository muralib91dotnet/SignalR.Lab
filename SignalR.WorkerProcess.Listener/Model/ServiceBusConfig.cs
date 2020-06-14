using System;
using System.Collections.Generic;
using System.Text;

namespace SignalR.WorkerProcess.Listener.Model
{
    public class ServiceBusConfig
    {
        public string FromEndpointQueue { get; set; }
        public string ToEndpointQueue { get; set; }

        public string ServiceBusTransportString { get; set; }

        public int ImmediateNumberOfRetries { get; set; }
        public int DelayedNumberOfRetries { get; set; }
        public int DelayTimeSpanInSeconds { get; set; }
    }
}
