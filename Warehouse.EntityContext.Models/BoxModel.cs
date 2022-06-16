using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.EntityContext.Models
{
    [Table("Box")]
    [Index(nameof(Id), Name = "Id")]
    public partial class BoxModel
    {
        [Key]
        [Required]
        public int? Id { get; set; }

        [Required]
        public int? Length { get; set; }

        [Required]
        public int? Width { get; set; }

        [Required]
        public int? Height { get; set; }

        [Required]
        public int? Weight { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ExpirationDate { get; set; }

        [Column(name:"PalletId")]
        public int? PalletModelId { get; set; }
        public PalletModel? PalletModel { get; set; }
    }
}
