using Application.Exceptions;
using Domains.DTO;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Interfaces.IUnitofWork;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
namespace Application.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<OrderDetailResponseDTO>> GetOrderDetailsAsync()
        {
            var orderDetails = await _unitOfWork.GetGenericRepository<OrderDetail>().GetAllAsync();

            // Project order details into OrderDetailResponseDTO records
            var orderDetailResponse = orderDetails.Select(o => new OrderDetailResponseDTO
            (
                o.ProductId,
                o.Product.Name,
                o.Quantity,
                o.Product.Price,
                o.Quantity * o.Product.Price,
                o.OrderId
            ));

            // Convert the IEnumerable<OrderDetailResponseDTO> to a List and return
            return orderDetailResponse.ToList(); // Use ToList() instead of ToListAsync()
        }


        public async Task<OrderDetailFromOrderIdResponseDTO> GetOrderDetailByOrderIdAsync(int id)
        {
            if (id == 0)
            {
                throw new NotFoundException("Order Detail not found");
            }
            else
            {
                var orderDetail = await _unitOfWork.GetGenericRepository<OrderDetail>().GetQueryable()
                    .Include(x => x.Product) // this is eager loading the Product navigation property 
                    .Include(x => x.Order) // this is eager loading the Order navigation property
                    .ThenInclude(order => order.Customer) // this is eager loading the Customer navigation property
                    .SingleOrDefaultAsync(x => x.OrderId == id);

                if (orderDetail == null)
                {
                    throw new NotFoundException("Order Detail not found");
                }

                return new OrderDetailFromOrderIdResponseDTO
                (
                    ProductId: orderDetail.ProductId,
                    Quantity: orderDetail.Quantity,
                    UnitPrice: orderDetail.Product.Price,
                    ProductName: orderDetail.Product.Name,
                    OrderId: orderDetail.OrderId,
                    TotalPrice: orderDetail.Quantity * orderDetail.Product.Price,
                    CustomerName: orderDetail.Order.Customer.FullName,
                    CustomerEmail: orderDetail.Order.Customer.Email,
                    OrderDate: orderDetail.Order.OrderDate,
                    CustomerId: orderDetail.Order.CustomerId
                );
            }
        }

    }
}
