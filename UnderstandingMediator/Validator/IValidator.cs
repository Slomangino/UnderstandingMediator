using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnderstandingMediator.Message;

namespace UnderstandingMediator.Validator
{
    public interface IValidator<TMessage, TResult> where TMessage : IMessage<TResult>
    {
        Task<string> Validate(TMessage message);
    }
}
