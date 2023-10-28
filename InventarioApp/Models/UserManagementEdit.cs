using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventarioApp.Models
{
    public class UserManagementEdit
    {
        [Key]
        [ReadOnly(true)]
        [Required]
        public required string Id { get; set; }


        [DisplayName("Rol")]
        public required string Role { get; set; }
    }
}
