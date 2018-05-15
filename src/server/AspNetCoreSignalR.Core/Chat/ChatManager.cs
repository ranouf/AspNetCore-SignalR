using AspNetCoreSignalR.Common.Dependancies;
using AspNetCoreSignalR.Common.Events;
using AspNetCoreSignalR.Core.Chat.Entities;
using AspNetCoreSignalR.Core.Chat.Events;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSignalR.Core.Chat
{
    public class ChatManager : BaseManager, IChatManager
    {
        public IList<User> Users { get; } = new List<User>();

        public ChatManager(ILogger<ChatManager> logger, IDomainEvents domainEvents) : base(logger, domainEvents)
        {
        }

        public async Task<User> FindUserByIdAsync(string userId)
        {
            Logger.LogInformation($"BEGIN - try to find a user by Id: '{userId}'.");
            var result = GetUsers().FirstOrDefault(u => u.Id == userId);
            if (result == null)
            {
                await Events.RaiseAsync(new ErrorEvent() { Message = $"'{userId}' not found." });
                Logger.LogInformation($"FAILED - '{userId}' doesn't exist.");
                return null;
            }
            Logger.LogInformation($"END - User '{result.Name}' match with Id: '{userId}'.");
            return result;
        }

        public IEnumerable<User> GetUsers()
        {
            return Users;
        }

        public async Task JoinAsync(User user)
        {
            Logger.LogInformation($"BEGIN - '{user.Name}' try to join the chat.");

            if (Users.Any(u => u.Id == user.Id))
            {
                await Events.RaiseAsync(new ErrorEvent()
                {
                    Message = "Same Id already exists.",
                    Recipient = user
                });
                Logger.LogInformation($"FAILED - '{user.Id}' can't join the chat because same Id already exists.");
                return;
            }

            if (Users.Any(u => u.Name == user.Name))
            {
                await Events.RaiseAsync(new ErrorEvent()
                {
                    Message = "Same Name already exists.",
                    Recipient = user
                });
                Logger.LogInformation($"FAILED - '{user.Name}' can't join the chat because same Name already exists.");
                return;
            }

            Users.Add(user);
            await Events.RaiseAsync(new UserJoinedEvent() { User = user, Users = GetUsers() });
            Logger.LogInformation($"END - '{user.Name}' has joined the chat.");
        }

        public async Task LeaveAsync(User user)
        {
            Logger.LogInformation($"BEGIN - '{user.Name}' try to leave the chat.");
            Users.Remove(user);
            await Events.RaiseAsync(new UserLeftEvent() { User = user, Users = GetUsers() });
            Logger.LogInformation($"END - '{user.Name}' has left the chat.");
        }

        public async Task<Message> SendMessageToAllAsync(Message message)
        {
            Logger.LogInformation($"BEGIN - '{message.Author.Name}' try to send a message.");
            await Events.RaiseAsync(new MessageSentToAllEvent() { Message = message });
            Logger.LogInformation($"END - '{message.Author.Name}' sent a message.");
            return message;
        }

        public async Task<PrivateMessage> SendPrivateMessageAsync(PrivateMessage privateMessage)
        {
            Logger.LogInformation($"BEGIN - '{privateMessage.Author.Name}' try to send a private message.");
            await Events.RaiseAsync(new PrivateMessageSentEvent() { PrivateMessage = privateMessage });
            Logger.LogInformation($"END - '{privateMessage.Author.Name}' sent a private message.");
            return privateMessage;
        }
    }
}
