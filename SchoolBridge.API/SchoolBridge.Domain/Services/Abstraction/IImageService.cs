using SchoolBridge.DataAccess.Entities.Files.Images;
using SchoolBridge.Helpers.Managers.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IImageService: IOnInitService, IOnFirstInitService
    {
        Task<string> Add(DataImage base64Image);
        Task<IEnumerable<string>> Add(IEnumerable<DataImage> base64Images);
        Task<string> Add(string base64Image);
        Task<DataImage> Get(string Id);
        Task Remove(string Id);
        Task Remove(Image image);
    }
}