﻿namespace AspNetCoreSignalR.Core.Chat.Entities
{
    public class User
    {
        public string Id { get; }
        public string Name { get; }

        public User(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
