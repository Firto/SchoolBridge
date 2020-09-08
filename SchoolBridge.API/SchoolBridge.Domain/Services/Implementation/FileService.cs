using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.DataAccess.Entities.Files;
using SchoolBridge.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;

namespace SchoolBridge.Domain.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly IGenericRepository<File> _fileGR;
        private readonly FileServiceConfiguration _configuration;

        public FileService(IGenericRepository<File> fileGR,
                            FileServiceConfiguration configuration)
        {
            _fileGR = fileGR;
            _configuration = configuration;      
        }

        public static void OnInit(ClientErrorManager manager, FileServiceConfiguration configuration)
        {
            manager.AddErrors(new ClientErrors("FileService", new Dictionary<string, ClientError>() {
                    {"too-big-file", new ClientError($"Too big file > {configuration.MaxSize} byte" ) },
                    {"file-load-err", new ClientError("File loading error") }
            }));
        }

        public static void OnFirstInit(IGenericRepository<File> fileGR) {
            fileGR.Create(
              new File { Id = "default-user-photo" }
            );
        }

        public string CreatePath(string fileName) 
            => _configuration.SaveDirectory + "/" + fileName;

        public async Task<string> Add(string source) {
            if (source.Length > _configuration.MaxSize)
                throw new ClientException("too-big-file");
            File ph = await _fileGR.CreateAsync(new File());
            try
            {
                System.IO.File.WriteAllText(CreatePath(ph.Id), source);
            }
            catch (System.Exception)
            {
                await _fileGR.DeleteAsync(ph);
                if (System.IO.File.Exists(CreatePath(ph.Id)))
                    System.IO.File.Delete(CreatePath(ph.Id));
                throw new ClientException("file-load-err");
            }
            return ph.Id;
        }

        public async Task<IEnumerable<string>> Add(IEnumerable<string> sources)
        {
            return null;
        }

        public async Task<string> Get(string Id)
        {
            if (await _fileGR.FindAsync(Id) == null || !System.IO.File.Exists(CreatePath(Id)))
                return null;

            return await System.IO.File.ReadAllTextAsync(CreatePath(Id));
        }

        public async Task Remove(string Id) {
            File file = await _fileGR.FindAsync(Id);
            await Remove(file);
        }

        public async Task RemoveUnSave(string Id)
        {
            await _fileGR.DeleteAsync(new File { Id = Id} );
            if (System.IO.File.Exists(CreatePath(Id)))
                System.IO.File.Delete(CreatePath(Id));
        }

        public async Task Remove(File file)
        {
            if (file != null)
            {
                await _fileGR.DeleteAsync(file);
                if (System.IO.File.Exists(CreatePath(file.Id)))
                    System.IO.File.Delete(CreatePath(file.Id));
            }
        }
    }
}