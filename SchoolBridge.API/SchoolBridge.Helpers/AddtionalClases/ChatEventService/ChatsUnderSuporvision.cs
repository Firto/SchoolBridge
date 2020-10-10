using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using SchoolBridge.Helpers.Extentions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.ChatEventService
{
    public class ChatUserSession
    {
        private ConcurrentDictionary<string/*chat id*/, ChatUnderSupervision> _chats = new ConcurrentDictionary<string, ChatUnderSupervision>();
        public IDictionary<string, ChatUnderSupervision> Chats { get => _chats; }

        public readonly UserSession BaseSession;
        public string ConnectionId { get => BaseSession.ConnectionId; }
        public string UserId { get => BaseSession.UserId; }
        public ChatUserSession(UserSession baseSession) 
        {
            BaseSession = baseSession;
        }
        public void AddChat(ChatUnderSupervision chat) 
        {
            if (_chats.ContainsKey(chat.ChatId))
                throw new Exception("Chat is already exists");

            if (!_chats.TryAdd(chat.ChatId, chat))
                throw new Exception("Error while adding chat to user");
        }

        public void RemoveChat(ChatUnderSupervision chat) 
        {
            if (!_chats.ContainsKey(chat.ChatId))
                throw new Exception("Chat isn't exists");

            if (!_chats.TryRemove(chat.ChatId, out chat))
                throw new Exception("Error while removing chat");
        }
    }

    public class SubscribedUser : IDisposable
    {
        private ConcurrentDictionary<string/*connection id*/, ChatUserSession> _sessions = new ConcurrentDictionary<string, ChatUserSession>();
        private long _lastType = DateTime.Now.ToUnixTimestamp();
        public IDictionary<string, ChatUserSession> Sessions { get => _sessions; }
        public readonly string UserId;

        public long LastType { get => _lastType; }
        public SubscribedUser(ChatUserSession baseSession) {
            //AddSession(baseSession);
            UserId = baseSession.UserId;
        }

        public int GetCountSessions()
            => _sessions.Count;
        public bool IsIssetSession(ChatUserSession session)
            => _sessions.ContainsKey(session.ConnectionId);

        public void AddSession(ChatUserSession session)
            => _sessions.AddOrUpdate(session.ConnectionId, session, (x, y) => session);

        public void RemoveSession(ChatUserSession session)
        {
            ChatUserSession ses;
            if (!_sessions.Remove(session.ConnectionId, out ses))
                throw new Exception("Error while removing session");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChatUserSession> GetSessions()
            => _sessions.Values;

        public void UpdateLastType()
            => _lastType = DateTime.Now.ToUnixTimestamp();

        public void SetNotTyping()
            => _lastType = DateTime.Now.ToUnixTimestamp()-10;
    }
    public class ChatUnderSupervision : IDisposable
    {
        // users
        private ConcurrentDictionary<string/*user id*/, SubscribedUser> _users = new ConcurrentDictionary<string, SubscribedUser>();
        public IDictionary<string, SubscribedUser> Users { get => _users; }
        public readonly string ChatId;
        public ChatUnderSupervision(string chatId) {
            ChatId = chatId;
        }

        public void Subscribe(ChatUserSession session) {
            if (!_users.ContainsKey(session.UserId) && !_users.TryAdd(session.UserId, new SubscribedUser(session)))
                throw new Exception("Error add chat user session");
            _users[session.UserId].AddSession(session);
        }

        public void Unsubscribe(ChatUserSession session) 
        {
            if (!_users.ContainsKey(session.UserId))
                throw new Exception("None chat user session");

            _users[session.UserId].RemoveSession(session);

            SubscribedUser user;
            if (_users[session.UserId].Sessions.Count == 0 && !_users.Remove(session.UserId, out user))
                throw new Exception("Error remove chat user session");
        }

        public IEnumerable<ChatUserSession> GetSessions()
        {
            var arr = new List<ChatUserSession>();
            _users.Values.ForEach(x => arr.AddRange(x.GetSessions()));
            return arr;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    public class ChatsUnderSupervision : IDisposable
    {
        // chats
        private ConcurrentDictionary<string/*chat id*/, ChatUnderSupervision> _chats = new ConcurrentDictionary<string, ChatUnderSupervision>();
        private ConcurrentDictionary<string/*connection id*/, ChatUserSession> _users = new ConcurrentDictionary<string, ChatUserSession>();

        public IDictionary<string/*chat id*/, ChatUnderSupervision> Chats { get => _chats; }
        public IDictionary<string/*connection id*/, ChatUserSession> Users { get => _users; }

        private ChatUserSession ConnectUser(UserSession session) 
        {
            if (_users.ContainsKey(session.ConnectionId))
                throw new Exception("User already connected");

            if (!_users.TryAdd(session.ConnectionId, new ChatUserSession(session)))
                    throw new Exception("Eroor connecting user to chat");
            return _users[session.ConnectionId];
        }

        private ChatUserSession DisconnectUser(UserSession session)
        {
            if (!_users.ContainsKey(session.ConnectionId))
                throw new Exception("User not connected");
            ChatUserSession o;
            if (!_users.TryRemove(session.ConnectionId, out o))
                throw new Exception("Error disconnecting user of chat");
            return o;
        }

        public ChatUserSession GetUser(UserSession session) {
            if (!_users.ContainsKey(session.ConnectionId))
                throw new Exception("User not connected");
            return _users[session.ConnectionId];
        }

        public ChatUserSession GetOrAddUser(UserSession session) {
            if (!_users.ContainsKey(session.ConnectionId) && !_users.TryAdd(session.ConnectionId, new ChatUserSession(session)))
                    throw new Exception("Eroor connecting user to chat");
            return _users[session.ConnectionId];
        }

        private ChatUnderSupervision ConnectChat(string chatId) 
        {
            if (_chats.ContainsKey(chatId))
                throw new Exception("Chat supervision already exists");

            if (!_chats.TryAdd(chatId, new ChatUnderSupervision(chatId)))
                throw new Exception("Eroor adding chat to supervision");

            return _chats[chatId];
        }

        private ChatUnderSupervision DisconnectChat(string chatId)
        {
            if (!_chats.ContainsKey(chatId))
                throw new Exception("Chat supervision not exists");
            ChatUnderSupervision o;
            if (!_chats.TryRemove(chatId, out o))
                throw new Exception("Eroor removing chat of supervision");

            return o;
        }

        public void SmartDisconnectChat(string chatId) 
        {
            if (!_chats.ContainsKey(chatId))
                throw new Exception("Chat supervision not exists");
            ChatUnderSupervision o;
            if (_chats[chatId].Users.Count == 0 && !_chats.TryRemove(chatId, out o))
                throw new Exception("Eroor removing chat of supervision");
        }

        private ChatUnderSupervision GetOrAddChat(string chatId)
        {
            if (!_chats.ContainsKey(chatId) && !_chats.TryAdd(chatId, new ChatUnderSupervision(chatId)))
                throw new Exception("Eroor connecting chat");
            return _chats[chatId];
        }

        private void SubscribeUserToChat(string chatId, ChatUserSession session)
        {
            var som = GetOrAddChat(chatId);
            som.Subscribe(session);
            if (!session.Chats.ContainsKey(chatId))
                session.AddChat(som);
        }        

        public void Subscribe(string chatId, UserSession session) 
        {
            SubscribeUserToChat(chatId, GetOrAddUser(session));
        }

        public void Unsubscribe(string chatId, UserSession session) { 
            if (!_chats.ContainsKey(chatId))
                throw new Exception("Chat supervision not exists");

            var som = GetUser(session);
            som.RemoveChat(_chats[chatId]);
            _chats[chatId].Unsubscribe(som);
            SmartDisconnectChat(chatId);
        }

        public void FullDisconnectUser(UserSession session) {
            var som = DisconnectUser(session);
            som.Chats.Values.ForEach(x => {
                x.Unsubscribe(som);
                SmartDisconnectChat(x.ChatId);
            });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
