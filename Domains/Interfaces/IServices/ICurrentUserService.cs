using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Interfaces.IServices
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Username { get; }
        string[] Roles { get; }
    }
}
