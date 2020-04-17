using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolBridge.Domain.Services.Abstraction;

namespace SchoolBridge.API.Controllers
{
    [Route("api/[controller]/")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
            => _imageService = imageService;

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            if (Id == null)
                return NotFound();
            var image = await _imageService.Get(Id);
            if (image == null)
                return NotFound();
            return File(image.RawData, image.MimeType);

        }
    }
}