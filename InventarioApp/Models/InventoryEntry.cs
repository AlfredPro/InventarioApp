using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarioApp.Models
{
    public class InventoryEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ReadOnly(true)]
        [Required]
        public required int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Nombre")]
        public required string Name { get; set; }

        [StringLength(50)]
        [DisplayName("Tipo")]
        public required string Type { get; set; }

        [DisplayName("Descripción")]
        public string? Description { get; set; }

        [DisplayName("Notas")]
        public string? Notes { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Cantidad")]
        public required int Quantity { get; set; }
    }
}
