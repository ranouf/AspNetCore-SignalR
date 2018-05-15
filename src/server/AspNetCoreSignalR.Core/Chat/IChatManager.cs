using AspNetCoreSignalR.Core.Chat.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreSignalR.Core.Chat
{
    public interface IChatManager
    {
        Task<User> FindUserByIdAsync(string userId);
        IEnumerable<User> GetUsers();
        Task JoinAsync(User user);
        Task LeaveAsync(User user);
        Task<Message> SendMessageToAllAsync(Message message);
        Task<PrivateMessage> SendPrivateMessageAsync(PrivateMessage privateMessage);
    }
}
