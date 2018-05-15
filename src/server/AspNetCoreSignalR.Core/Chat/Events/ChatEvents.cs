using AspNetCoreSignalR.Common.Events;
using AspNetCoreSignalR.Core.Chat.Entities;
using System.Collections.Generic;

namespace AspNetCoreSignalR.Core.Chat.Events
{
    public class ChatEvent : IEvent
    {
    }

    public class MessageSentToAllEvent : ChatEvent
    {
        public Message Message { get; set; }
    }

    public class PrivateMessageSentEvent : ChatEvent
    {
        public PrivateMessage PrivateMessage { get; set; }
    }

    public class UserJoinedEvent : ChatEvent
    {
        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }
    }

    public class UserLeftEvent : ChatEvent
    {
        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }
    }

    public class ErrorEvent : ChatEvent
    {
        public string Message { get; set; }
        public User Recipient { get; set; }
    }
}
