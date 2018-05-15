using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSignalR.Api.Hubs
{
    public class BaseHub : Hub
    {
        public IMapper Mapper { get; }
        public ILogger Logger { get; }

        public BaseHub(
            IMapper mapper,
            ILogger logger
        )
        {
            Mapper = mapper;
            Logger = logger;
        }
    }
}