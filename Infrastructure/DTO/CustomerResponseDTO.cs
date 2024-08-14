using System;

namespace Infrastructure.DTO
{
    public record CustomerResponseDTO
    (
        int CustomerId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Address
    );
}
