using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnderstandingMediator
{
    public interface IValidator<TMessage, TResult> where TMessage : IMessage<TResult>
    {
        Task<string> Validate(TMessage message);
    }
}
