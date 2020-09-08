using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using SchoolBridge.Helpers.Extentions;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using SchoolBridge.Helpers.DtoModels.Globalization;
using System.Security.Cryptography.X509Certificates;
using SchoolBridge.Helpers.Managers;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class LanguageStringService : ILanguageStringService
    {
        private readonly IGenericRepository<Language> _languaesGR;
        private readonly IGenericRepository<LanguageString> _languageStringGR;
        private readonly IGenericRepository<LanguageStringType> _languageStringTypeGR;
        private readonly IGenericRepository<LanguageStringIdType> _languageStringIdTypeGR;
        private readonly IGenericRepository<LanguageStringId> _languageStringIdGR;
        private readonly HttpContext _httpContext;
        private LanguageStringServiceConfiguration _configuration = null;

        private string _langAddName = null;
        
        public LanguageStringService(IGenericRepository<Language> languaesGR,
                                        IGenericRepository<LanguageString> languageStringGR,
                                        IGenericRepository<LanguageStringType> languageStringTypeGR,
                                        IGenericRepository<LanguageStringIdType> languageStringIdTypeGR,
                                        IGenericRepository<LanguageStringId> languageStringIdGR,
                                        LanguageStringServiceConfiguration configuration, 
                                        ScopedHttpContext scopedHttpContext)
        {
            _languaesGR = languaesGR;
            _languageStringGR = languageStringGR;
            _languageStringIdTypeGR = languageStringIdTypeGR;
            _languageStringTypeGR = languageStringTypeGR;
            _languageStringIdGR = languageStringIdGR;
            _configuration = configuration;
            _httpContext = scopedHttpContext.HttpContext;

            if (_httpContext.Request.Headers.ContainsKey("accept-language") && ((String)_httpContext.Request.Headers["accept-language"]).Length == 2 ) {
                _langAddName = _httpContext.Request.Headers["accept-language"];
            }

            if (_langAddName == null || _languaesGR.CountWhere(x => x.AbbName == _langAddName) == 0)
                _langAddName = _configuration.DefaultLanguage;


            /*foreach (var item in arguments)
                draftBody = draftBody.ReplaceFirstOccurrance("$arg$", item);
            return draftBody;*/

            // var m = _languageStringGR.GetDbSet().Where((x) => x.Id == Id && x.Language.AbbName == langAddName).Include(x => x.Language).ToArray().FirstOrDefault();
        }

        public static void OnInit(ClientErrorManager manager, IValidatingService validatingService, LanguageStringServiceConfiguration configuration)
        {
            manager.AddErrors(new ClientErrors("LanguageStringService", new Dictionary<string, ClientError>
                {
                    { "lss-inc-lang-str", new ClientError("Incorrect language string!")},
                    { "lss-lng-ald-reg", new ClientError("Language already registered!")},
                    { "lss-lng-no-reg", new ClientError("Incorrect language!") },
                    { "lss-type-ald-reg", new ClientError("Type already registered!")},
                    { "lss-type-no-reg", new ClientError("Incorrect type!")},
                    { "lss-strid-ald-reg", new ClientError("String id already registered!")},
                    { "lss-strid-no-reg", new ClientError("Incorrect string id!")},
                    { "lss-baseupateid-inc", new ClientError("Incorrect base update id!")}
                }));

            validatingService.AddValidateFunc("lss-lng-name", (string som, PropValidateContext ctx) => {
                if (som == null) return;
                else if (som.Length != 2)
                    ctx.Valid.Add($"[lss-inc-lang-str]");
            });

            validatingService.AddValidateFunc("lss-lng-type", (string som, PropValidateContext ctx) => {
                if (som == null) return;
                else if (som.Length > configuration.MaxTypeNameLength)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {configuration.MaxTypeNameLength}]"); //"Too long login(max {_configuration.MaxCountCharsLogin} characters)!"
            });

            validatingService.AddValidateFunc("lss-str-id-name", (string som, PropValidateContext ctx) => {
                if (som == null) return;
                else if (som.Length > configuration.MaxStringIdNameLength)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {configuration.MaxStringIdNameLength}]");
            });

            validatingService.AddValidateFunc("lss-str-name", (string som, PropValidateContext ctx) => {
                if (som == null) return;
                else if (som.Length > configuration.MaxStringLength)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {configuration.MaxStringLength}]");
            });

            validatingService.AddValidateFunc("lss-lng-full-name", (string som, PropValidateContext ctx) => {
                if (som == null) return;
                else if (som.Length > configuration.MaxLangFullNameLength)
                    ctx.Valid.Add($"[str-too-long, [pn-{ctx.PropName}], {configuration.MaxLangFullNameLength}]");
            });
        }

        public static void OnFirstInit(IGenericRepository<Language> language, IGenericRepository<LanguageStringType> languageStringTypeGR)
        {
            languageStringTypeGR.Create(new LanguageStringType { Name = "default" });

            language.Create(new Language[] {
                    new Language { AbbName = "en", FullName = "English" },
                    new Language { AbbName = "ua", FullName = "Українська" }
            });
        }

        public string GetUpdateId() {
            return _configuration.BaseUpdId;
        }

        public GlobalizationInfoDto GetGlobalizationInfo() {
            return new GlobalizationInfoDto {
                CurrentLanguage = _langAddName,
                AwaibleLanguages = GetLanguages().ToDictionary(x => x.AbbName, x => x.FullName),
                BaseUpdateId = _configuration.BaseUpdId
            };
        }

        public string UpdateBaseUpdateId() {
            _configuration.BaseUpdId = Guid.NewGuid().ToString();
            return _configuration.BaseUpdId;
        }

        public Language AddLanguage(string abbName, string fullName) {  /// NO VALIDATION
            if (_languaesGR.CountWhere(x => x.AbbName == abbName.ToLower()) > 0)
                throw new ClientException("lss-lng-ald-reg");

            return _languaesGR.Create(new Language { AbbName = abbName.ToLower(), FullName = fullName });
        }

        public void RemoveLanguage(string abbName)
        {  /// NO VALIDATION
            if (_languaesGR.CountWhere(x => x.AbbName == abbName.ToLower()) == 0)
                throw new ClientException("lss-lng-no-reg");

            _languaesGR.Delete(x => x.AbbName == abbName.ToLower());
        }

        public void RemoveLanguage(int id)
        {  /// NO VALIDATION
            var m = _languaesGR.Find(id);
            if (m == null)
                throw new ClientException("lss-lng-no-reg");

            _languaesGR.Delete(m);
        }

        public void EditLanguage(string abbName, string fullName) {
            var languages = _languaesGR.GetAll(x => x.AbbName == abbName.ToLower());
            if (languages.Count() == 0)
                throw new ClientException("lss-lng-no-reg");
            var language = languages.FirstOrDefault();
            language.FullName = fullName;

            _languaesGR.Update(language);
        }

        public LanguageStringType AddType(string name)
        {  /// NO VALIDATION
            if (_languageStringTypeGR.CountWhere(x => x.Name == name) > 0)
                throw new ClientException("lss-type-ald-reg");

            return _languageStringTypeGR.Create(new LanguageStringType { Name = name });
        }

        public void RemoveType(string name)
        {  /// NO VALIDATION
            if (_languageStringTypeGR.CountWhere(x => x.Name == name) == 0)
                throw new ClientException("lss-type-no-reg");

            _languageStringTypeGR.Delete(x => x.Name == name);
        }

        public void RemoveType(int id)
        {  /// NO VALIDATION
            var m = _languageStringTypeGR.Find(id);
            if (m == null)
                throw new ClientException("lss-type-no-reg");

            _languageStringTypeGR.Delete(m);
        }

        public LanguageStringId AddStringId(string name, int[] typesIds)
        {  /// NO VALIDATION
            if (_languageStringIdGR.CountWhere(x => x.Name == name) > 0)
                throw new ClientException("lss-strid-ald-reg");

            return _languageStringIdGR.Create(new LanguageStringId { Name = name, Types = typesIds.Select(x => new LanguageStringIdType { TypeId = x}).ToList() });
        }

        public LanguageStringId AddStringId(string name, string[] types)
        {  /// NO VALIDATION
            if (_languageStringIdGR.CountWhere(x => x.Name == name) > 0)
                throw new ClientException("lss-strid-ald-reg");
            var s = _languageStringTypeGR.GetAll(x => types.Contains(x.Name));
            if (s.Count() != types.Length)
                throw new ClientException("lss-type-no-reg", types);
            return _languageStringIdGR.Create(new LanguageStringId { Name = name, Types = s.Select(x => new LanguageStringIdType { TypeId = x.Id }).ToList() });
        }

        public void RemoveStringId(string name)
        {  /// NO VALIDATION
            if (_languageStringIdGR.CountWhere(x => x.Name == name) == 0)
                throw new ClientException("lss-type-no-reg");

            _languageStringIdGR.Delete(x => x.Name == name);
        }

        public void RemoveStringId(int id)
        {  /// NO VALIDATION
            var m = _languageStringIdGR.Find(id);
            if (m == null)
                throw new ClientException("lss-type-no-reg");

            _languageStringIdGR.Delete(m);
        }

        public LanguageString AddString(int IdId, int langId, string str)
        {  /// NO VALIDATION
            if (_languageStringIdGR.Find(IdId) == null)
                throw new ClientException("lss-strid-no-reg");
            else if (_languaesGR.Find(langId) == null)
                throw new ClientException("lss-lng-no-reg");

            return _languageStringGR.Create(new LanguageString { IdId = IdId, LanguageId = langId, String = str });
        }

        public LanguageString AddString(string idName, string langAbbName, string str)
        {  /// NO VALIDATION
            var s = _languageStringIdGR.GetAll(x => x.Name == idName).FirstOrDefault();
            if (s == null)
                throw new ClientException("lss-strid-no-reg");
            var l = _languaesGR.GetAll(x => x.AbbName == langAbbName).FirstOrDefault();
            if (l == null)
                throw new ClientException("lss-lng-no-reg");

            return _languageStringGR.Create(new LanguageString { IdId = s.Id, LanguageId = l.Id, String = str });
        }

        /*public IEnumerable<LanguageString> AddOrUpdateString(string idName, string[] types, IDictionary<string, string> strs) {
            var l = _languaesGR.GetAll(x => strs.Keys.Contains(x.AbbName)).ToDictionary(k => k.Id, v => v);

            if (l.Count() != strs.Count)
                throw new ClientException("lss-lng-no-reg", strs.Keys.Except(l.Values.Select(x => x.AbbName)));

            var r = _languageStringIdGR.GetDbSet().Where(x => x.Name == idName).Include(x => x.Types).ThenInclude(x => x.Type).FirstOrDefault();
            if (r == null)
            {
                var s = _languageStringTypeGR.GetAll(x => types.Contains(x.Name));
                if (s.Count() != types.Length)
                    throw new ClientException("lss-type-no-reg", types);
                _languageStringIdGR.Create(new LanguageStringId { Name = idName, Types = s.Select(x => new LanguageStringIdType { TypeId = x.Id }).ToList() });
                r = _languageStringIdGR.GetDbSet().Where(x => x.Name == idName).Include(x => x.Types).ThenInclude(x => x.Type).FirstOrDefault();
            }

            if (types.Intersect(r.Types.Select(x => x.Type.Name)).Count() != types.Length)
            {
                var s = _languageStringTypeGR.GetAll(x => types.Contains(x.Name));
                if (s.Count() != types.Length)
                    throw new ClientException("lss-type-no-reg", types);

                _languageStringIdTypeGR.GetDbSet().RemoveRange(r.Types);
                _languageStringIdTypeGR.GetDbSet().AddRange(s.Select(x => new LanguageStringIdType { TypeId = x.Id, StringIdId = r.Id }));
                _languageStringIdTypeGR.GetDbContext().SaveChanges();
            }

            var m = _languageStringGR.GetAll(x => x.IdId == r.Id && l.ContainsKey(x.LanguageId)).TakeWhile(br => l.ContainsKey(br.LanguageId) && l[br.LanguageId])
        }*/

        public LanguageString AddOrUpdateString(string idName, string[] types, string langAbbName, string str) 
        {
            var l = _languaesGR.GetAll(x => x.AbbName == langAbbName).FirstOrDefault();
            if (l == null)
                throw new ClientException("lss-lng-no-reg");

            var r = _languageStringIdGR.GetDbSet().Where(x => x.Name == idName).Include(x => x.Types).ThenInclude(x => x.Type).FirstOrDefault();
            if (r == null) {
                var s = _languageStringTypeGR.GetAll(x => types.Contains(x.Name));
                if (s.Count() != types.Length)
                    throw new ClientException("lss-type-no-reg", types);
                _languageStringIdGR.Create(new LanguageStringId { Name = idName, Types = s.Select(x => new LanguageStringIdType { TypeId = x.Id }).ToList() });
                r = _languageStringIdGR.GetDbSet().Where(x => x.Name == idName).Include(x => x.Types).ThenInclude(x => x.Type).FirstOrDefault();
            }

            if (types.Intersect(r.Types.Select(x => x.Type.Name)).Count() != types.Length)
            {
                var s = _languageStringTypeGR.GetAll(x => types.Contains(x.Name));
                if (s.Count() != types.Length)
                    throw new ClientException("lss-type-no-reg", types);

                _languageStringIdTypeGR.GetDbSet().RemoveRange(r.Types);
                _languageStringIdTypeGR.GetDbSet().AddRange(s.Select(x => new LanguageStringIdType { TypeId = x.Id, StringIdId = r.Id }));
                _languageStringIdTypeGR.GetDbContext().SaveChanges();
            }

            var m = _languageStringGR.GetAll(x => x.IdId == r.Id && x.LanguageId == l.Id).FirstOrDefault();

            if (m == null)
                return _languageStringGR.Create(new LanguageString { IdId = r.Id, LanguageId = l.Id, String = str });

            m.String = str;
            _languageStringGR.Update(m);

            return m;
        }

        public IDictionary<string, string> GetByType(string langAddName, int[] typeIds)
        {
            return _languageStringGR.GetDbSet()
                .Where(x => x.Language.AbbName == langAddName)
                .Include(x => x.Language)
                .Include(x => x.Id)
                .ThenInclude(x => x.Types)
                .ToArray()
                .TakeWhile(x => x.Id.Types.FirstOrDefault(r => typeIds.Contains(r.TypeId)) != null)
                .ToDictionary(k => k.Id.Name, v => v.String);
        }

        public IDictionary<string, string> GetByType(int langId, int[] typeIds)
        {
            return _languageStringGR.GetDbSet()
                .Where(x => x.LanguageId == langId)
                .Include(x => x.Id)
                .ThenInclude(x => x.Types)
                .ToArray()
                .TakeWhile(x => x.Id.Types.FirstOrDefault(r => typeIds.Contains(r.TypeId)) != null)
                .ToDictionary(k => k.Id.Name, v => v.String);
        }

        public IDictionary<string, string> GetByType(string langAddName, string[] types)
        {
            return _languageStringGR.GetDbSet()
                                      .Where(x => x.Language.AbbName == langAddName)
                                      .Include(x => x.Language)
                                      .Include(x => x.Id)
                                      .ThenInclude(x => x.Types)
                                      .ThenInclude(x => x.Type)
                                      .ToArray()
                                      .Where(s => s.Id.Types.Select(x => x.Type.Name).Any(types.Contains))
                                      .ToDictionary(k => k.Id.Name, v => v.String);
        }

        public IDictionary<string, string> GetByType(int langId, string[] types)
        {
            return _languageStringGR.GetDbSet()
                .Where(x => x.LanguageId == langId)
                .Include(x => x.Id)
                .ThenInclude(x => x.Types)
                .ThenInclude(x => x.Type)
                .ToArray()
                .TakeWhile(x => x.Id.Types.FirstOrDefault(r => types.Contains(r.Type.Name)) != null)
                .ToDictionary(k => k.Id.Name, v => v.String);
        }

        public IDictionary<string, IDictionary<string, string>> GetByType(string[] langAddNames, int[] typeIds)
        {
            return _languaesGR.GetDbSet()
                              .Where(x => langAddNames.Contains(x.AbbName))
                              .Include(x => x.Strings)
                              .ThenInclude(x => x.Id)
                              .ThenInclude(x => x.Types)
                              .ToArray()
                              .TakeWhile(x => x.Strings.FirstOrDefault(s => s.Id.Types.FirstOrDefault(m => typeIds.Contains(m.TypeId)) != null) != null)
                              .ToDictionary(k => k.AbbName, v => (IDictionary<string, string>)v.Strings.ToDictionary(kk => kk.Id.Name, vv => vv.String));
        }

        public IDictionary<string, IDictionary<string, string>> GetByType(int[] langIds, int[] typeIds)
        {
            return _languaesGR.GetDbSet()
                              .Where(x => langIds.Contains(x.Id))
                              .Include(x => x.Strings)
                              .ThenInclude(x => x.Id)
                              .ThenInclude(x => x.Types)
                              .ToArray()
                              .TakeWhile(x => x.Strings.FirstOrDefault(s => s.Id.Types.FirstOrDefault(m => typeIds.Contains(m.TypeId)) != null) != null)
                              .ToDictionary(k => k.AbbName, v => (IDictionary<string, string>)v.Strings.ToDictionary(kk => kk.Id.Name, vv => vv.String));
        }

        public IDictionary<string, IDictionary<string, string>> GetByType(string[] langAddNames, string[] types)
        {
            return _languaesGR.GetDbSet()
                              .Where(x => langAddNames.Contains(x.AbbName))
                              .Include(x => x.Strings)
                              .ThenInclude(x => x.Id)
                              .ThenInclude(x => x.Types)
                              .ThenInclude(x => x.Type)
                              .ToArray()
                              .TakeWhile(x => x.Strings.FirstOrDefault(s => s.Id.Types.FirstOrDefault(m => types.Contains(m.Type.Name)) != null) != null)
                              .ToDictionary(k => k.AbbName, v => (IDictionary<string, string>)v.Strings.ToDictionary(kk => kk.Id.Name, vv => vv.String));
        }

        public IDictionary<string, IDictionary<string, string>> GetByType(int[] langIds, string[] types)
        {
            return _languaesGR.GetDbSet()
                              .Where(x => langIds.Contains(x.Id))
                              .Include(x => x.Strings)
                              .ThenInclude(x => x.Id)
                              .ThenInclude(x => x.Types)
                              .ThenInclude(x => x.Type)
                              .ToArray()
                              .TakeWhile(x => x.Strings.FirstOrDefault(s => s.Id.Types.FirstOrDefault(m => types.Contains(m.Type.Name)) != null) != null)
                              .ToDictionary(k => k.AbbName, v => (IDictionary<string, string>)v.Strings.ToDictionary(kk => kk.Id.Name, vv => vv.String));
        }

        public Task<IDictionary<string, string>> GetByTypeAsync(string langAddName, int[] typeIds)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, string>> GetByTypeAsync(int langId, int[] typeIds)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, string>> GetByTypeAsync(string langAddName, string[] types)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, string>> GetByTypeAsync(int langId, string[] types)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, IDictionary<string, string>>> GetByTypeAsync(string[] langAddNames, int[] typeIds)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, IDictionary<string, string>>> GetByTypeAsync(int[] langIds, int[] typeIds)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, IDictionary<string, string>>> GetByTypeAsync(string[] langAddNames, string[] types)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, IDictionary<string, string>>> GetByTypeAsync(int[] langIds, string[] types)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetByTypeCurrent(int[] typeIds)
        {
            return GetByType(_langAddName, typeIds);
        }

        public IDictionary<string, string> GetByTypeCurrent(string[] types)
        {
            return GetByType(_langAddName, types);
        }

        public Task<IDictionary<string, string>> GetByTypeCurrentAsync(int[] typeIds)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, string>> GetByTypeCurrentAsync(string[] types)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetByTypeDefault(int[] typeIds)
        {
            return GetByType(_configuration.DefaultLanguage, typeIds);
        }

        public IDictionary<string, string> GetByTypeDefault(string[] types)
        {
            return GetByType(_configuration.DefaultLanguage, types);
        }

        public Task<IDictionary<string, string>> GetByTypeDefaultAsync(int[] typeIds)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, string>> GetByTypeDefaultAsync(string[] types)
        {
            throw new NotImplementedException();
        }

        /*private string ComposeLanguageString(string str, params string[] args) {
            foreach (var item in args)
                str = str.ReplaceFirstOccurrance("$arg$", item);
            return str;
        }*/

        public IEnumerable<Language> GetLanguages()
            => _languaesGR.GetAll();

        public async Task<IEnumerable<Language>> GetLanguagesAsync()
            => await _languaesGR.GetAllAsync();

        public IEnumerable<LanguageStringType> GetLanguageStringTypes()
            => _languageStringTypeGR.GetAll();

        public async Task<IEnumerable<LanguageStringType>> GetLanguageStringTypesAsync()
            => await _languageStringTypeGR.GetAllAsync();
    }
}
