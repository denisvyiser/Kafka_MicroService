using BBari.Application.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.Application.Interfaces
{
    public interface IAppMessageService
    {
        void MessageProducerService(NewMessage message);
        NewMessage MessageConsumererService();
        
    }
}
