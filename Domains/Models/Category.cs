using Domains.Models.BaseEntity;
using Domains.Models.BridgeEntity;

namespace Domains.Models
{
    public class Category: Entity
    {
        public string CategoryName { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

    }
}
