using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.EntityContext.Models
{
    [Table("Pallet")]
    [Index(nameof(Id), Name = "Id")]
    public partial class PalletModel
    {
        [Key]
        public int? Id { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public ICollection<BoxModel>? Boxes{ get; set; }
    }
}
