using System.Collections.Generic;

namespace AspNetCoreSignalR.Api.Hubs.Dtos
{
    public class UsersDto
    {
        public UserDto User { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}
