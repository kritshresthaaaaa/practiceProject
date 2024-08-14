using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class ProductStockDTO
    {
        public int ProductId { get; set; }
        public int RemainingStock { get; set; }
    }

}
