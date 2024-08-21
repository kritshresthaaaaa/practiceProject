using Domains.DTO;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Interfaces.IServices
{
    public interface IUserValidator
    {
        User ValidateUser(LoginRequestDTO loginRequestDTO);
    }
}
