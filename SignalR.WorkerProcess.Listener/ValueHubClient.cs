using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.WorkerProcess.Listener
{
    public class ValueHubClient
    {
        private readonly IConfiguration _configuration;

        private HubConnection _hubConnection;

        public ValueHubClient(IConfiguration configuration)
        {
            _configuration = configuration;

            _hubConnection = new HubConnectionBuilder()
                            .WithUrl(configuration.GetValue<string>("SignalRHubUrl"))
                            .Build();
        }

        // TODO: Refactor HubConnection with methods to be passed from callee
        public async Task StartHubConnection(Dictionary<string, Action<object>> hubFunctions)
        {
            
            foreach(var hubFunction in hubFunctions)
            {
                _hubConnection.On(hubFunction.Key, hubFunction.Value);
            }

            await _hubConnection.StartAsync();
        }

    }
}
