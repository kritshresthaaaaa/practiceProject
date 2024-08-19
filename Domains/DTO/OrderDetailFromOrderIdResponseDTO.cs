using System;

namespace Domains.DTO
{
    public record OrderDetailFromOrderIdResponseDTO
    (
        int ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice,
        int OrderId,
        DateTime OrderDate,
        int CustomerId,
        string CustomerName,
        string CustomerEmail
    );
}
