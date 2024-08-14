

using Domains.Models.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models
{
    public class Customer: Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }

}
