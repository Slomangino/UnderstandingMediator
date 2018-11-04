using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingMediator
{
    public interface IMessageDispatcher
    {
        Task<TResult> Dispatch<TResult>(IMessage<TResult> message);
    }
}
