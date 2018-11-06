using Autofac;
using Autofac.Core.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnderstandingMediator.Validator;

namespace UnderstandingMediator.Message
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IComponentContext _context;

        public MessageDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task<TResult> Dispatch<TResult>(IMessage<TResult> message)
        {
            var tMessageType = message.GetType();
            var tResultType = typeof(TResult);

            // invoke method dynamically
            MethodInfo method = typeof(MessageDispatcher).GetMethod("DispatchInternal");
            MethodInfo generic = method.MakeGenericMethod(tMessageType, tResultType);
            object result = generic.Invoke(this, new[] { message });

            return await(Task<TResult>)result;
        }

        public async Task<TResult> DispatchInternal<TMessage, TResult>(TMessage command)
            where TMessage : IMessage<TResult>
        {
            var validator = _context.ResolveOptional<IValidator<TMessage, TResult>>();
            var handler = _context.Resolve<IMessageHandler<TMessage, TResult>>();

            if (validator == null)
            {
                return await handler.Handle(command);
            }

            var validationResult = await validator.Validate(command);

            if (validationResult != string.Empty)
            {
                Console.WriteLine(string.Format("Validation for message {0} did not pass. Message: {1}", typeof(TMessage), validationResult));
            }

            return await handler.Handle(command);
        }
    }
}
