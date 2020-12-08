using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class LanguageStringIdType
    {
        [ForeignKey("StringId")]
        public int StringIdId { get; set; }
        public LanguageStringId StringId { get; set; }

        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public LanguageStringType Type { get; set; }
    }
}
