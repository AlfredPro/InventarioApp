using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventarioApp.Models
{
    public class UserManagement
    {
        [Key]
        [ReadOnly(true)]
        [Required]
        public required string Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Nombre")]
        public required string FirstName { get; set; }

        [StringLength(50)]
        [DisplayName("Apellido")]
        public required string LastName { get; set; }

        [DisplayName("Correo")]
        public required string Email { get; set; }

        [DisplayName("Rol")]
        public required string Role { get; set; }
    }
}
