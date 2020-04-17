using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SchoolBridge.Helpers.Managers.Image
{

    public class DataImage
    {
        private static readonly Regex _dataUriPattern = new Regex(@"^data\:(?<type>image\/(png|jpeg|jpg|gif));base64,(?<data>[A-Z0-9\+\/\=]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
        public DataImage(string mimeType, byte[] rawData)
        {
            MimeType = mimeType;
            RawData = rawData;
        }

        public string MimeType { get; set; }
        public byte[] RawData { get; set; }

        //public System.Drawing.Image Image {
        //    get {
        //        if (_image == null)
        //            _image = System.Drawing.Image.FromStream(new MemoryStream(RawData));
        //        return _image;
        //    }
        //}

        //public ImageProcessor.ImageFactory ImageFactory
        //{
        //    get
        //    {
        //        if (_imageFactory == null)
        //            _imageFactory = new ImageProcessor.ImageFactory();
        //        return _imageFactory;
        //    }
        //}

        //public MemoryStream MemStream
        //{
        //    get
        //    {
        //        if (_memoryStream == null)
        //            _memoryStream = new MemoryStream();
        //        return _memoryStream;
        //    }
        //}

        //private System.Drawing.Image _image = null;
        //private ImageProcessor.ImageFactory _imageFactory = null;
        //private MemoryStream _memoryStream = null;
        public static DataImage TryParse(string dataUri)
        {
            if (string.IsNullOrWhiteSpace(dataUri)) return null;

            Match match = _dataUriPattern.Match(dataUri);
            if (!match.Success) return null;

            string mimeType = match.Groups["type"].Value;
            string base64Data = match.Groups["data"].Value;

            try
            {
                byte[] rawData = Convert.FromBase64String(base64Data);
                return rawData.Length == 0 ? null : new DataImage(mimeType, rawData);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static bool IsImage(string dataUri)
        {
            if (string.IsNullOrWhiteSpace(dataUri)) return false;

            Match match = _dataUriPattern.Match(dataUri);
            if (!match.Success) return false;

            string base64Data = match.Groups["data"].Value;

            try
            {
                System.Drawing.Image som = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(base64Data)));
                return som != null;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool TryParse(string dataUri, out DataImage dataImage)
        {
            dataImage = null;

            if (string.IsNullOrWhiteSpace(dataUri)) return false;

            Match match = _dataUriPattern.Match(dataUri);
            if (!match.Success) return false;

            string mimeType = match.Groups["type"].Value;
            string base64Data = match.Groups["data"].Value;

            try
            {
                dataImage = new DataImage(mimeType, Convert.FromBase64String(base64Data));
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public override string ToString()
            => $"data:{MimeType};base64,{Convert.ToBase64String(RawData)}";

        public static string ToString(string data, string mimeType)
            => $"data:{mimeType};base64,{data}";

        public void Save(string filePath) {
            File.WriteAllText(filePath, ToString());
        }

        public static DataImage Load(string filePath)
        {
            return TryParse(File.ReadAllText(filePath));
        }

        //public void Constrain(System.Drawing.Size size) {
        //    MemStream.Clear();
        //    if (ImageFactory.Image == null)
        //        ImageFactory.Load(RawData);
        //    ImageFactory.Constrain(size).Save(MemStream);
        //    RawData = MemStream.ToArray();
        //}

        //public void Resize(System.Drawing.Size size)
        //{
        //    MemStream.Clear();
        //    if (ImageFactory.Image == null)
        //        ImageFactory.Load(RawData);
        //    ImageFactory.Resize(size).Save(MemStream);
        //    RawData = MemStream.ToArray();
        //}
    }
}
