using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSignalR.Api.Hubs
{
    public class ApiBase
    {
        public IMapper Mapper { get; }
        public ILogger Logger { get; }

        public ApiBase(
            IMapper mapper,
            ILogger logger
        )
        {
            Mapper = mapper;
            Logger = logger;
        }
    }
}
