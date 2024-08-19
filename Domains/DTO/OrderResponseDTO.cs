using System;

namespace Domains.DTO
{
    public record OrderResponseDTO
    (
        int Id,
        decimal TotalPrice,
        DateTime SaleDate,
        int ProductId,
        string ProductName
    );
}
