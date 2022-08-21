
namespace Warehouse.EntityContext.Entities
{
    public class BoxEntity
    {
        public int? Id { get; set; }

        public int? Length { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int? PalletModelId { get; set; }
        public PalletEntity? PalletModel { get; set; }
    }
}
