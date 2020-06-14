using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.RabbitMQ
{
    public class ExchangeSetting : BaseSetting
    {
        public string Type { get; set; } = Constants.DefaultDLXSetting;
    }
}
