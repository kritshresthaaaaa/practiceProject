using System;
using System.Collections.Generic;

namespace Infrastructure.DTO
{
    public record ProductResponseDTO
    (
        int Id,
        string Name,
        decimal Price,
        int StockQuantity,
        string Description,
        List<int> CategoryIds
    );
}
