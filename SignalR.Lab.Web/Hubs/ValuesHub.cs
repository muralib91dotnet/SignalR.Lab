using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Lab.Web.Hubs
{
    /// <summary>
    /// Used for providing realtime updates for the ValuesController.
    /// </summary>
    public class ValuesHub : Hub<IValuesClient>
    {
        /// <summary>
        /// Notify all users that a value has been added.
        /// </summary>
        /// <param name="value">The new value</param>
        public async Task Add(string value) => await Clients.All.Add(value);

        /// <summary>
        /// Notify all users that a value has been removed.
        /// </summary>
        /// <param name="value">The removed value</param>
        public async Task Delete(string value) => await Clients.All.Delete(value);
    }
}
// SignalR.WorkerProcess.Listener