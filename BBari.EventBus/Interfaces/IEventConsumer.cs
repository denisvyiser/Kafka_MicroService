using BBari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.EventBus.Interfaces
{
    public interface IEventConsumer<out T> where T : Message
    {
        T Handle();
    }
}
