using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.RabbitMQ
{
    public class BaseSetting
    {
        public string Name { get; set; }

        public bool IsDurable { get; set; } = false;

        public bool IsExclusive { get; set; } = false;

        public bool IsAutoDelete { get; set; } = false;
    }
}
