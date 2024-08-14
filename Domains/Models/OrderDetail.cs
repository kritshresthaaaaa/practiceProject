

using Domains.Models.BaseEntity;

namespace Domains.Models
{
    public class OrderDetail : Entity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        
    }
}
