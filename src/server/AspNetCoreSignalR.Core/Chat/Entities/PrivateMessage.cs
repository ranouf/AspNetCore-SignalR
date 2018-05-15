namespace AspNetCoreSignalR.Core.Chat.Entities
{
    public class PrivateMessage : Message
    {
        public User Recipient { get; set; }

        public PrivateMessage(string content, User author, User recipient) : base(content, author)
        {
            Recipient = recipient;
        }
    }
}
