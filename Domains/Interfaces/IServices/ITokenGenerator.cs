using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Interfaces.IServices
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
