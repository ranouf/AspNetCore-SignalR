namespace AspNetCoreSignalR.Api.Hubs.Dtos
{
    public class PrivateMessageDto : MessageDto
    {
        public UserDto Recipient { get; set; }
    }
}
