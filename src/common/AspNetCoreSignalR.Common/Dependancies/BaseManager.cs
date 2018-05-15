using AspNetCoreSignalR.Common.Events;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSignalR.Common.Dependancies
{
    public abstract class BaseManager : IManager
    {
        public ILogger Logger { get; }
        public IDomainEvents Events { get; }

        public BaseManager(
            ILogger logger,
            IDomainEvents domainEvents
        )
        {
            Logger = logger;
            Events = domainEvents;
        }
    }
}
