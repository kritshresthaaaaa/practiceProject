

using Domains.Models.BaseEntity;

namespace Domains.Models
{
    public class Order:Entity
    {
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
