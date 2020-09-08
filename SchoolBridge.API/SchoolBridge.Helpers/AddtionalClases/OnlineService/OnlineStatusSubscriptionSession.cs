using AI.Collections;
using SchoolBridge.Helpers.AddtionalClases.UserConnectionService;
using SchoolBridge.Helpers.Extentions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace SchoolBridge.Helpers.AddtionalClases.OnlineService
{
    public class OnlineStatusSubscriptionSession
    {
        public UserSession ClientSession { get; private set; }
        public IEnumerable<string> UsersUnderSupervision
        {
            get
            {
                return _allUsersUnderSupervision.Where(x => x.Value.ContainsKey(ClientSession.ConnectionId)).Select(x => x.Key);
            }
        }

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, OnlineStatusSubscription>> _allUsersUnderSupervision;

        public OnlineStatusSubscriptionSession(UserSession clientSession, ConcurrentDictionary<string, ConcurrentDictionary<string, OnlineStatusSubscription>> main) {
            ClientSession = clientSession;
            _allUsersUnderSupervision = main;
        }

        public bool IsUserUnderSupervision(string userId) {
            return _allUsersUnderSupervision.ContainsKey(userId) && _allUsersUnderSupervision[userId].ContainsKey(ClientSession.ConnectionId);
        }

        public bool AddUserToSupervision(OnlineStatusSubscription subscription)
        {
            if (!_allUsersUnderSupervision.ContainsKey(subscription.ClientUnderSupervisionId))
                _allUsersUnderSupervision.TryAdd(subscription.ClientUnderSupervisionId, new ConcurrentDictionary<string, OnlineStatusSubscription>());

            if (!_allUsersUnderSupervision[subscription.ClientUnderSupervisionId].ContainsKey(ClientSession.ConnectionId))
            {
                subscription.Session = this;
                return _allUsersUnderSupervision[subscription.ClientUnderSupervisionId].TryAdd(ClientSession.ConnectionId, subscription);
            }
            else return false;
        }

        public bool RemoveUserFromSupervision(string userId)
        {
            OnlineStatusSubscription session;
            if (_allUsersUnderSupervision.ContainsKey(userId))
            {
                if (_allUsersUnderSupervision[userId].TryRemove(ClientSession.ConnectionId, out session))
                {
                    ConcurrentDictionary<string, OnlineStatusSubscription> str;
                    if (_allUsersUnderSupervision[userId].Count == 0)
                        _allUsersUnderSupervision.TryRemove(userId, out str);
                    return true;
                }
                else return false;
            }
            return false;
        }

        public void ClearAllSubscriptions() {
            OnlineStatusSubscription session;
            ConcurrentDictionary<string, OnlineStatusSubscription> str;
            _allUsersUnderSupervision.Where(x => x.Value.ContainsKey(ClientSession.ConnectionId)).ForEach(x => {
                x.Value.TryRemove(ClientSession.ConnectionId, out session);
               
                ConcurrentDictionary<string, OnlineStatusSubscription> str;
                if (x.Value.Count == 0)
                    _allUsersUnderSupervision.TryRemove(x.Key, out str);
            });
        }
    }
}
