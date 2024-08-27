using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTO
{
    public class CategoryWithProductsResponseDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int TotalProducts { get; set; }
        public List<ProductResponseDTO> productResponseDTOs { get; set; }
    }
}
