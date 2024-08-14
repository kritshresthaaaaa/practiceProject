using Domains.Models.BaseEntity;
using Domains.Models.BridgeEntity;

namespace Domains.Models
{
    public class Product:Entity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

    }
}
