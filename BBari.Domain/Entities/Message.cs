using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.Domain.Entities
{
    public class Message
    {
        public Message(Guid id, string serviceId, string data, DateTime timeStamp)
        {
            Id = id;
            ServiceId = serviceId;
            Data = data;
            TimeStamp = timeStamp;
        }

        public Guid Id { get; protected set; }
        public string ServiceId { get; protected set; }
        public string Data { get; protected set; }

        public DateTime TimeStamp { get; protected set; }
    }
}
