using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSignalR.Common.Events
{
    public class DomainEvents : IDomainEvents
    {
        private readonly IComponentContext _container;
        private readonly ILogger<DomainEvents> _logger;

        public DomainEvents(IComponentContext container, ILogger<DomainEvents> logger)
        {
            _container = container;
            _logger = logger;
        }

        public Task RaiseAsync<T>(T args) where T : IEvent
        {
            var handlers = _container.Resolve<IEnumerable<IEventHandler<T>>>();

            return Task.Factory.StartNew(() => RaiseAction(handlers, args));
        }

        public void Raise<T>(T args) where T : IEvent
        {
            var handlers = _container.Resolve<IEnumerable<IEventHandler<T>>>();

            RaiseAction(handlers, args);
        }

        private void RaiseAction<T>(IEnumerable<IEventHandler<T>> handlers, T args) where T : IEvent
        {
            try
            {
                handlers.AsParallel().ForAll(handler => handler.Handle(args));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
