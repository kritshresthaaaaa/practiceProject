
using Domains.Models;
using Infrastructure.DTO;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using WebHost.Services.IServices;

namespace WebHost.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;

        public OrderDetailService(IGenericRepository<OrderDetail> orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }



        /*  public async Task<OrderDetailResponseDTO> GetOrderDetailByIdAsync(int id)
          {
              var orderDetail = await _orderDetailRepository.GetByIdAsync(id);

              return new OrderDetailResponseDTO
              {

                  ProductId = orderDetail.ProductId,
                  Quantity = orderDetail.Quantity,
                  UnitPrice = orderDetail.Product.Price,
                  ProductName = orderDetail.Product.Name,
                  OrderId = orderDetail.OrderId,
                  TotalPrice = orderDetail.Quantity * orderDetail.Product.Price,
              };
          }*/
        public async Task<IEnumerable<OrderDetailResponseDTO>> GetOrderDetailsAsync()
        {
            var orderDetails = await _orderDetailRepository.GetAllAsync();

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
                return null;
            }
            else
            {
                var orderDetail = await _orderDetailRepository.FirstOrDefaultAsync(
                    x => x.OrderId == id,
                    x => x.Product,
                    x => x.Order,
                    x => x.Order.Customer
                );

                if (orderDetail == null)
                {
                    return null;
                }

                return new OrderDetailFromOrderIdResponseDTO
                (
                    ProductId : orderDetail.ProductId,
                    Quantity : orderDetail.Quantity,
                    UnitPrice : orderDetail.Product.Price,
                    ProductName : orderDetail.Product.Name,
                    OrderId : orderDetail.OrderId,
                    TotalPrice : orderDetail.Quantity * orderDetail.Product.Price,
                    CustomerName : orderDetail.Order.Customer.FullName,
                    CustomerEmail : orderDetail.Order.Customer.Email,
                    OrderDate : orderDetail.Order.OrderDate,
                    CustomerId : orderDetail.Order.CustomerId
                );
            }
        }

    }
}
