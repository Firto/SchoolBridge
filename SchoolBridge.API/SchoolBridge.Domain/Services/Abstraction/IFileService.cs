using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IFileService: IOnInitService, IOnFirstInitService
    {
        Task<string> Add(string source);
        Task<IEnumerable<string>> Add(IEnumerable<string> sources);
        Task<string> Get(string Id);
        Task Remove(string Id);
        Task RemoveUnSave(string Id);
        string CreatePath(string fileName);
    }
}