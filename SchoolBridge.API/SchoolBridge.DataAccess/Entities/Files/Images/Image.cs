using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities.Files.Images
{
    public class Image
    {
        [ForeignKey("File")]
        public string FileId { get; set; }
        public File File { get; set; }
        [Required]
        public bool Static { get; set; } = false;
        [Required]
        public string Type { get; set; }
    }
}