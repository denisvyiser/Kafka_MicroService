using BBari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.EventBus.Interfaces
{
    public interface IEventProducer<in T> where T : Message
    {
        void Send(T data);
    }
}
