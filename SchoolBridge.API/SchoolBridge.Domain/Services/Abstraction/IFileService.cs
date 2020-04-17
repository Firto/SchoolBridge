using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IFileService
    {
        Task<string> Add(string source);
        Task<IEnumerable<string>> Add(IEnumerable<string> sources);
        Task<string> Get(string Id);
        Task Remove(string Id);
        Task RemoveUnSave(string Id);
        string CreatePath(string fileName);
    }
}