using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using SchoolBridge.Helpers.Extentions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.ChatEventService
{
    public class ChatEventSubscriptionSession
    {
        public UserSession ClientSession { get; private set; }
        public IEnumerable<string> ChatsUnderSupervision
        {
            get
            {
                return _allUsersUnderSupervision.Where(x => x.Value.ContainsKey(ClientSession.ConnectionId)).Select(x => x.Key);
            }
        }

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ChatEventSubscription>> _allUsersUnderSupervision;

        public ChatEventSubscriptionSession(UserSession clientSession, ConcurrentDictionary<string, ConcurrentDictionary<string, ChatEventSubscription>> main)
        {
            ClientSession = clientSession;
            _allUsersUnderSupervision = main;
        }

        public bool IsChatUnderSupervision(string userId)
        {
            return _allUsersUnderSupervision.ContainsKey(userId) && _allUsersUnderSupervision[userId].ContainsKey(ClientSession.ConnectionId);
        }

        public bool AddChatToSupervision(ChatEventSubscription subscription)
        {
            if (!_allUsersUnderSupervision.ContainsKey(subscription.ChatUnderSupervisionId))
                _allUsersUnderSupervision.TryAdd(subscription.ChatUnderSupervisionId, new ConcurrentDictionary<string, ChatEventSubscription>());

            if (!_allUsersUnderSupervision[subscription.ChatUnderSupervisionId].ContainsKey(ClientSession.ConnectionId))
            {
                subscription.Session = this;
                return _allUsersUnderSupervision[subscription.ChatUnderSupervisionId].TryAdd(ClientSession.ConnectionId, subscription);
            }
            else return false;
        }

        public bool RemoveChatFromSupervision(string userId)
        {
            ChatEventSubscription session;
            if (_allUsersUnderSupervision.ContainsKey(userId))
            {
                if (_allUsersUnderSupervision[userId].TryRemove(ClientSession.ConnectionId, out session))
                {
                    ConcurrentDictionary<string, ChatEventSubscription> str;
                    if (_allUsersUnderSupervision[userId].Count == 0)
                        _allUsersUnderSupervision.TryRemove(userId, out str);
                    return true;
                }
                else return false;
            }
            return false;
        }

        public void ClearAllSubscriptions()
        {
            ChatEventSubscription session;
            ConcurrentDictionary<string, ChatEventSubscription> str;
            _allUsersUnderSupervision.Where(x => x.Value.ContainsKey(ClientSession.ConnectionId)).ForEach(x => {
                x.Value.TryRemove(ClientSession.ConnectionId, out session);

                ConcurrentDictionary<string, ChatEventSubscription> str;
                if (x.Value.Count == 0)
                    _allUsersUnderSupervision.TryRemove(x.Key, out str);
            });
        }
    }
}
