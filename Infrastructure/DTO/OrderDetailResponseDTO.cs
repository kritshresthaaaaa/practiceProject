using System;

namespace Infrastructure.DTO
{
    public record OrderDetailResponseDTO
    (
        int ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice,
        int OrderId
    );
}
