using System.Threading.Tasks;

namespace AspNetCoreSignalR.Common.Events
{
    public interface IDomainEvents
    {
        Task RaiseAsync<T>(T args) where T : IEvent;
    }
}
