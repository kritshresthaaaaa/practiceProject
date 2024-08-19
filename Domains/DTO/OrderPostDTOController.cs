
using Domains.DTO;

namespace Domains.DTO
{
    public class OrderPostDTOController
    {

        public int CustomerId { get; set; }
        public List<ProductStockDTO> OrderDetailsWithProductRemaingStock { get; set; }
    }
}
