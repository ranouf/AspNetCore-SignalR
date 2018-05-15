using AspNetCoreSignalR.Api.Hubs.Dtos;
using AspNetCoreSignalR.Common.Events;
using AspNetCoreSignalR.Core.Chat;
using AspNetCoreSignalR.Core.Chat.Entities;
using AspNetCoreSignalR.Core.Chat.Events;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreSignalR.Api.Hubs
{
    public class ChatHub : BaseHub
    {
        private readonly IChatManager _chatManager;

        public ChatHub(
            IChatManager chatManager,
            IMapper mapper,
            ILogger logger
            ) : base(mapper, logger)
        {
            _chatManager = chatManager;
        }

        [HubMethodName("Join")]
        public async Task JoinAsync(string name)
        {
            await _chatManager.JoinAsync(new User(Context.ConnectionId, name));
        }

        [HubMethodName("Leave")]
        public async Task LeaveAsync(string name)
        {
            await _chatManager.LeaveAsync(new User(Context.ConnectionId, name));
        }

        [HubMethodName("SendMessageToAll")]
        public async Task SendMessageToAllAsync(string content)
        {
            var author = await _chatManager.FindUserByIdAsync(Context.ConnectionId);
            await _chatManager.SendMessageToAllAsync(new Message(content, author));
        }

        [HubMethodName("SendPrivateMessage")]
        public async Task SendPrivateMessage(string content, string recipientId)
        {
            var author = await _chatManager.FindUserByIdAsync(Context.ConnectionId);
            var recipient = await _chatManager.FindUserByIdAsync(recipientId);
            await _chatManager.SendPrivateMessageAsync(new PrivateMessage(content, author, recipient));
        }
    }

    public class ChatHubHandler :
        ApiBase,
        IEventHandler<MessageSentToAllEvent>,
        IEventHandler<PrivateMessageSentEvent>,
        IEventHandler<UserJoinedEvent>,
        IEventHandler<UserLeftEvent>,
        IEventHandler<ErrorEvent>
    {
        private readonly IHubContext<ChatHub> _connectionManager;

        public const string MessageSentToAll = "MessageSentToAll";
        public const string PrivateMessageSent = "PrivateMessageSent";
        public const string UserJoined = "UserJoined";
        public const string UserLeft = "UserLeft";
        public const string Error = "Error";

        public ChatHubHandler(
            IHubContext<ChatHub> connectionManager,
            IMapper mapper,
            ILogger<ChatHub> logger
        ) : base(mapper, logger)
        {
            _connectionManager = connectionManager;
        }

        public void Handle(MessageSentToAllEvent args)
        {
            Logger.LogInformation($"SIGNALR - {MessageSentToAll} - Message:'{args.Message.Content}' by '{args.Message.Author.Name}'.");
            var result = Mapper.Map<Message, MessageDto>(args.Message);
            _connectionManager.Clients.All.SendAsync(MessageSentToAll, JsonConvert.SerializeObject(result));
        }

        public void Handle(PrivateMessageSentEvent args)
        {
            Logger.LogInformation($"SIGNALR - {PrivateMessageSent} - Message:'{args.PrivateMessage.Content}' by '{args.PrivateMessage.Author.Name}' to  by '{args.PrivateMessage.Recipient.Name}'.");
            var result = Mapper.Map<PrivateMessage, PrivateMessageDto>(args.PrivateMessage);
            _connectionManager.Clients.User(args.PrivateMessage.Recipient.Id).SendAsync(PrivateMessageSent, JsonConvert.SerializeObject(result));
        }

        public void Handle(UserJoinedEvent args)
        {
            Logger.LogInformation($"SIGNALR - {UserJoined} - {args.User.Name} has joined the chat.");
            var result = new UsersDto()
            {
                User = Mapper.Map<User, UserDto>(args.User),
                Users = Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(args.Users)
            };
            _connectionManager.Clients.All.SendAsync(UserJoined, JsonConvert.SerializeObject(result));
        }

        public void Handle(UserLeftEvent args)
        {
            Logger.LogInformation($"SIGNALR - {UserLeft} - {args.User.Name} has left the chat.");
            var result = new UsersDto()
            {
                User = Mapper.Map<User, UserDto>(args.User),
                Users = Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(args.Users)
            };
            _connectionManager.Clients.All.SendAsync(UserLeft, JsonConvert.SerializeObject(result));
        }

        public void Handle(ErrorEvent args)
        {
            Logger.LogInformation($"SIGNALR - {Error} - Message: '{args.Message}' for '{args.Recipient.Name}'.");
            var result = new ErrorDto()
            {
                Message = args.Message,
                Recipient = Mapper.Map<User, UserDto>(args.Recipient)
            };
            _connectionManager.Clients.User(args.Recipient.Id).SendAsync(UserLeft, JsonConvert.SerializeObject(result));
        }
    }

}
