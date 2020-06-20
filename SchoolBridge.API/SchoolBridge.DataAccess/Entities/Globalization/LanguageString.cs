using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class LanguageString
    {
        [ForeignKey("Id")]
        public int IdId { get; set; }
        public LanguageStringId Id { get; set; }

        [ForeignKey("Language")]
        public int LanguageId { get; set; }
        public Language Language { get; set; }

        [Required]
        public string String { get; set; }
    }
}
