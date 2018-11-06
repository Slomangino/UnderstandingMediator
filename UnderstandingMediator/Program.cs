using Autofac;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UnderstandingMediator.Message;

namespace UnderstandingMediator
{
    class Program
    {
        public static void Main(string[] args)
        {
            Main();
        }
        static async Task Main()
        {
            var builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();

            // Do assembly registrations by ending name.
            builder.RegisterAssemblyTypes(assembly)
                .Where(a => a.Name.EndsWith("Handler"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assembly)
               .Where(a => a.Name.EndsWith("Validator"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            // Register our Dispatcher
            builder.RegisterType<MessageDispatcher>().AsImplementedInterfaces().InstancePerLifetimeScope();

            var container = builder.Build();

            var messageDispatcher = container.Resolve<IMessageDispatcher>();

            var message = new ValidatedMessage(21);
            // Will throw validation exception if value is over 20
            var result = await messageDispatcher.Dispatch(message);

            var message3 = new NonValidatedMessage(4);
            // Will skip validation because there is no validator for this type
            var result3 = await messageDispatcher.Dispatch(message3);

            Console.WriteLine(result);
            Console.WriteLine(result3);

            Console.ReadKey();
        }
    }

    public class ValidatedMessage : IMessage<bool>
    {
        public int value { get; private set; }
        public ValidatedMessage(int val)
        {
            value = val;
        }
    }

    public class ValidatedMessageHandler : IMessageHandler<ValidatedMessage, bool>
    {
        public ValidatedMessageHandler(){}

        public Task<bool> Handle(ValidatedMessage message)
        {
            return Task.FromResult(message.value > 10);
        }
    }

    public class NonValidatedMessage : IMessage<bool>
    {
        public int value { get; private set; }
        public NonValidatedMessage(int val)
        {
            value = val;
        }
    }

    public class NonValidatedMessageHandler : IMessageHandler<NonValidatedMessage, bool>
    {
        public NonValidatedMessageHandler(){}

        public Task<bool> Handle(NonValidatedMessage message)
        {
            return Task.FromResult(message.value > 10);
        }
    }
}
