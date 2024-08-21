using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTO
{
    public record LoginResponseDTO(
        string Token,
        string UserId,
        string Username
    );
}
