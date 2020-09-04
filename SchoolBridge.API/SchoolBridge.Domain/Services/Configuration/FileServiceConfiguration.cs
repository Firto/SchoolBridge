using SchoolBridge.Domain.Services.Abstraction;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class FileServiceConfiguration: IMyService
    {
        // Byte // 100 mb
        public uint MaxSize { get; set; } = 100 * 1000000;
        public string SaveDirectory { get; set; }
    }
}