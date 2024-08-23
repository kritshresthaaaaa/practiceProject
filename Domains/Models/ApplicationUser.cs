using Microsoft.AspNetCore.Identity;
using System;

namespace Domains.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
