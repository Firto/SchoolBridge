using System.ComponentModel.DataAnnotations;

namespace SchoolBridge.DataAccess.Entities
{
    public class Panel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}