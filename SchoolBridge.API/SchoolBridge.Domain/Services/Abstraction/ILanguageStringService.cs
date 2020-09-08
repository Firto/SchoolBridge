using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Helpers.DtoModels.Globalization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface ILanguageStringService: IOnInitService, IOnFirstInitService
    {
        GlobalizationInfoDto GetGlobalizationInfo();
        string GetUpdateId();
        string UpdateBaseUpdateId();

        Language AddLanguage(string abbName, string fullName);
        void RemoveLanguage(string abbName);
        void RemoveLanguage(int id);
        void EditLanguage(string abbName, string fullName);

        LanguageStringType AddType(string name);
        void RemoveType(string name);
        void RemoveType(int id);

        LanguageStringId AddStringId(string name, int[] typesIds);
        LanguageStringId AddStringId(string name, string[] types);

        void RemoveStringId(string name);
        void RemoveStringId(int id);

        LanguageString AddString(int IdId, int langId, string str);
        LanguageString AddString(string idName, string langAbbName, string str);
        LanguageString AddOrUpdateString(string idName, string[] types, string langAbbName, string str);

        IEnumerable<Language> GetLanguages();
        IEnumerable<LanguageStringType> GetLanguageStringTypes();
        IDictionary<string, string> GetByType(string langAddName, int[] typeIds);
        IDictionary<string, string> GetByType(int langId, int[] typeIds);
        IDictionary<string, string> GetByType(string langAddName, string[] types);
        IDictionary<string, string> GetByType(int langId, string[] types);

        IDictionary<string, IDictionary<string, string>> GetByType(string[] langAddNames, int[] typeIds);
        IDictionary<string, IDictionary<string, string>> GetByType(int[] langIds, int[] typeIds);
        IDictionary<string, IDictionary<string, string>> GetByType(string[] langAddNames, string[] types);
        IDictionary<string, IDictionary<string, string>> GetByType(int[] langIds, string[] types);

        IDictionary<string, string> GetByTypeCurrent(int[] typeIds);
        IDictionary<string, string> GetByTypeCurrent(string[] types);

        IDictionary<string, string> GetByTypeDefault(int[] typeIds);
        IDictionary<string, string> GetByTypeDefault(string[] types);

        Task<IEnumerable<Language>> GetLanguagesAsync();
        Task<IEnumerable<LanguageStringType>> GetLanguageStringTypesAsync();

        Task<IDictionary<string, string>> GetByTypeAsync(string langAddName, int[] typeIds);
        Task<IDictionary<string, string>> GetByTypeAsync(int langId, int[] typeIds);
        Task<IDictionary<string, string>> GetByTypeAsync(string langAddName, string[] types);
        Task<IDictionary<string, string>> GetByTypeAsync(int langId, string[] types);

        Task<IDictionary<string, IDictionary<string, string>>> GetByTypeAsync(string[] langAddNames, int[] typeIds);
        Task<IDictionary<string, IDictionary<string, string>>> GetByTypeAsync(int[] langIds, int[] typeIds);
        Task<IDictionary<string, IDictionary<string, string>>> GetByTypeAsync(string[] langAddNames, string[] types);
        Task<IDictionary<string, IDictionary<string, string>>> GetByTypeAsync(int[] langIds, string[] types);

        Task<IDictionary<string, string>> GetByTypeCurrentAsync(int[] typeIds);
        Task<IDictionary<string, string>> GetByTypeCurrentAsync(string[] types);

        Task<IDictionary<string, string>> GetByTypeDefaultAsync(int[] typeIds);
        Task<IDictionary<string, string>> GetByTypeDefaultAsync(string[] types);
    }
}
