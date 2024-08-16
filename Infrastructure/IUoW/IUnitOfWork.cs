using Domains.Models;
using Infrastructure.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderDetail> OrderDetails { get; }
        IGenericRepository<Customer> Customers { get; }
        Task<int> SaveAsync();

    }
}
