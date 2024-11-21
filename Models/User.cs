using System.ComponentModel.DataAnnotations.Schema;

namespace SoftWizard.Models
{
    [Table("Users")]
    public class User
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("NameUser")]
        public required string NameUser { get; set; }

        [Column("Email")]
        public required string Email { get; set; }
    }
}