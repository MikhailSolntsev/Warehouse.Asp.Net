
namespace Warehouse.EntityContext.Entities
{
    public class PalletEntity
    {
        public int? Id { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public ICollection<BoxEntity>? Boxes{ get; set; }
    }
}
