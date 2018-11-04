using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingMediator
{
    public interface IMessageHandler<TMessage, TResult> where TMessage : IMessage<TResult>
    {
        Task<TResult> Handle(TMessage message);
    }
}
