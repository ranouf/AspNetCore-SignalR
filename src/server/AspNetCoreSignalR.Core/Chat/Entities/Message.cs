namespace AspNetCoreSignalR.Core.Chat.Entities
{
    public class Message
    {
        public string Content { get; }
        public User Author { get; }

        public Message(string content, User author)
        {
            Content = content;
            Author = author;
        }
    }
}
