using System;

namespace Infrastructure.DTO
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
