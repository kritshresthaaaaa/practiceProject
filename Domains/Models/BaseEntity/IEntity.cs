
namespace Domains.Models.BaseEntity
{
    public interface IEntity
    {
        DateTime CreatedDate { get; set; }
        int Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}