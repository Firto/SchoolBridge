using ImageProcessor.Imaging.Formats;
using System.Drawing;
using SchoolBridge.Domain.Services.Abstraction;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class ImageServiceConfiguration: IMyService
    {
        public FormatBase Format { get; set; } = new JpegFormat();
        public Size MaxSize { get; set; } = new Size(1280, 1280);
        // Byte // 10 mb
        public uint MaxSizeByte { get; set; } = 10 * 1000000;
    }
}