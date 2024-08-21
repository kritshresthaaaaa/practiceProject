using Domains.Interfaces.IGenericRepository;
using Domains.Models.BaseEntity;
using System;
using System.Threading.Tasks;

namespace Domains.Interfaces.IUnitofWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetGenericRepository<T>() where T : Entity;
        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
