using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSignalR.Api.Hubs.Dtos
{
    public class MessageDto
    {
        public string Content { get; set; }
        public UserDto Author { get; set; }
    }
}
