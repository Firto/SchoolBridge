using SchoolBridge.Helpers.DtoModels.ClientErrors.Info;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolBridge.Helpers.Managers.CClientErrorManager
{
    public class ClientErrors
    {
        protected readonly static Dictionary<string, ClientError> _errors = new Dictionary<string, ClientError>();
        protected readonly List<string> _clientErrors = new List<string>();
        protected readonly List<ClientErrors> _childLists = new List<ClientErrors>();
        public string DictionaryName { get; private set; }

        public ClientErrors() { }

        public ClientErrors(string DictionaryName, Dictionary<string, ClientError> Child = null, List<ClientErrors> ChildLists = null)
        {
            this.DictionaryName = DictionaryName;
            if (Child != null)
                foreach (var value in Child)
                    AddError(value.Key, value.Value);
            if (ChildLists != null)
                foreach (var item in ChildLists)
                    AddErrors(item);
        }

        public bool IsIssetErrors(string DictionaryName) {
            for (int i = 0; i < _childLists.Count; i++)
                if (_childLists[i].DictionaryName == DictionaryName || _childLists[i].IsIssetErrors(DictionaryName))
                    return true;
            return false;
        }

        public void AddError(string id, ClientError error)
        {
            if (!_errors.TryAdd(id, error))
                throw new Exception("Icorrect error ID!");
            else _clientErrors.Add(id);
        }

        public void AddError(string id, string decription)
            => AddError(id, new ClientError(decription));

        public void AddError(string id, ClientError error, params string[] path) {
            if (path == null || path.Length == 0)
                AddError(id, error);

            ClientErrors some = _childLists.FirstOrDefault((x) => x.DictionaryName == path[0]);

            if (some != null)
                some.AddError(id, error, path.Skip(1).ToArray());
            else throw new Exception("Icorrect dictionary name!");
        }

        public void AddError(string id, string decription, params string[] path) 
            => AddError(id, new ClientError(decription), path);

        public void AddErrors(ClientErrors errors)
            => _childLists.Add(errors);

        public virtual ClientBaseErrorsInfoDto GetInfo()
        {
            ClientErrorsInfoDto dto = new ClientErrorsInfoDto { Name = DictionaryName };
            if (_clientErrors.Count > 0)
            {
                dto.Errors = new Dictionary<string, ClientErrorInfoDto>();
                foreach (var item in _clientErrors)
                    dto.Errors.Add(item, GetErrorByID(item).GetInfo());
            }
            if (_childLists.Count > 0)
            {
                var set = new List<ClientBaseErrorsInfoDto>();
                foreach (var item in _childLists)
                    set.Add(item.GetInfo());
                dto.Dictionaries = set;
            }
            return dto;
        }

        public static string GetId(ClientError error)
            => _errors.First((x) => x.Value == error).Key;

        public static ClientError GetErrorByID(string Id)
            => _errors.FirstOrDefault((x) => x.Key == Id).Value;
    }
}