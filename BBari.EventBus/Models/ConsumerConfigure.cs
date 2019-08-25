using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.EventBus.Models
{
    public class ConsumerConfigure
    {
    public string GroupIdget { get; set; }
    public string BootstrapServers { get; set; }
    public string AutoOffsetReset { get; set; }
    public bool EnableAutoCommit { get; set; }
    }
}
