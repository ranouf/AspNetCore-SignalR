namespace AspNetCoreSignalR.Api.Hubs.Dtos
{
    public class ErrorDto
    {
        public string Message { get; set; }
        public UserDto Recipient { get; set; }
    }
}
