using Infrastructure.DTO;

namespace WebHost.DTO
{
    public class OrderPostDTOController
    {

        public int CustomerId { get; set; }
        public List<ProductStockDTO> OrderDetailsWithProductRemaingStock { get; set; }
    }
}
