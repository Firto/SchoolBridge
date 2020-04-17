using AutoMapper;
using SchoolBridge.DataAccess.Entities.Files.Images;
using SchoolBridge.DataAccess.Interfaces;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.Managers.CClientErrorManager;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using SchoolBridge.Helpers.Managers.Image;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IGenericRepository<Image> _imageGR;
        private readonly IMapper _mapper;
        private readonly ImageServiceConfiguration _configuration;
        private readonly IFileService _fileService;
        public ImageService(IGenericRepository<Image> imageGR,
                            IMapper mapper,
                            ImageServiceConfiguration configuration,
                            IFileService fileService,
                            ClientErrorManager clientErrorManager)
        {
            _imageGR = imageGR;
            _mapper = mapper;
            _configuration = configuration;
            _fileService = fileService;

            if (!clientErrorManager.IsIssetErrors("Image"))
                clientErrorManager.AddErrors(new ClientErrors("Image", new Dictionary<string, ClientError> {
                    { "inc-photo-type", new ClientError("Incorrect photo type!") },
                    { "image-load-err", new ClientError("Image loading error!") },
                    { "too-big-image", new ClientError($"Too big image > {configuration.MaxSizeByte} byte" ) },
                }));
        }

        public async Task<string> Add(DataImage base64Image) {
            if (base64Image.RawData.Length > _configuration.MaxSizeByte)
                throw new ClientException("too-big-image");
            else if (base64Image == null)
                throw new ClientException("inc-photo-type", base64Image.ToString());
            string Id = null;
            MemoryStream ms = new MemoryStream();
            try
            {
                new ImageProcessor.ImageFactory()
                    .Load(base64Image.RawData)
                    .Constrain(_configuration.MaxSize)
                    .Format(_configuration.Format)
                    .Save(ms);
                Id = await _fileService.Add(Convert.ToBase64String(ms.ToArray()));
                await _imageGR.CreateAsync(new Image { FileId = Id, Type = _configuration.Format.MimeType });
            }
            catch (Exception)
            {
                if (Id != null)
                    await _fileService.Remove(Id);
                throw new ClientException("image-load-err");
            }
            finally
            {
                ms.Close();
            }
            return Id;
        }

        public Task<IEnumerable<string>> Add(IEnumerable<DataImage> base64Images)
        {
            throw new ClientException("image-load-err");
        }

        public async Task<string> Add(string base64Image)
            => await Add(DataImage.TryParse(base64Image));

        public async Task<DataImage> Get(string Id)
        {
            Image img = await _imageGR.FindAsync(Id);
            if (img == null)
                return null;

            return new DataImage(img.Type, Convert.FromBase64String(await File.ReadAllTextAsync(_fileService.CreatePath(Id))));
        }

        public async Task Remove(string Id) 
        {
            Image photo = await _imageGR.FindAsync(Id);
            await Remove(photo);
        }

        public async Task Remove(Image image)
        {
            if (image != null && !image.Static)
                await _fileService.RemoveUnSave(image.FileId);
        }

    }
}