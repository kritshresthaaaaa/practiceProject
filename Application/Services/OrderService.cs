﻿using Application.Exceptions;
using Domains.DTO;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IServices;
using Domains.Models;
using Microsoft.EntityFrameworkCore;


namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public OrderService(IGenericRepository<Order> saleRepository, IGenericRepository<Product> productRepository)
        {
            _orderRepository = saleRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderResponseDTO> CreateSaleAsync(OrderPostDTOController orderDto)
        {

            var newOrder = new Order
            {
                OrderDate = DateTime.UtcNow,
                CustomerId = orderDto.CustomerId,
                OrderDetails = new List<OrderDetail>()
            };

            decimal totalPrice = 0;
            foreach (var detail in orderDto.OrderDetailsWithProductRemaingStock)
            {
                // Create OrderDetail
                var orderDetail = new OrderDetail
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.RemainingStock,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow

                };

                newOrder.OrderDetails.Add(orderDetail);
            }


            await _orderRepository.AddAsync(newOrder);


            return new OrderResponseDTO
            (
                Id: newOrder.Id,
                TotalPrice: totalPrice + newOrder.OrderDetails.Sum(d => d.Quantity * d.Product.Price),
                SaleDate: newOrder.OrderDate,
                ProductId: newOrder.OrderDetails.First().ProductId, // Example, you might want to customize this
                ProductName: newOrder.OrderDetails.First().Product.Name // Again, this is a simplification
            );
        }



        public async Task<OrderResponseDTO> GetSaleByIdAsync(int id)
        {
            var sales = await _orderRepository.GetAllAsync();

            // eager loading
            // include is used to include the related data in the query, here we are including the order details and the product details of the order details
            //theninclude is used to include the related data of the related data, here we are including the product details of the order details
            var sale = sales.Include(s => s.OrderDetails).ThenInclude(d => d.Product).FirstOrDefault(s => s.Id == id); // here we are including the order details and the product details of the order details and this is done to avoid lazy loading
            // if we didnt do early loading, the order details and the product details of the order details would be loaded lazily, which means that they would be loaded when they are accessed, this would result in multiple queries to the database, which is not efficient
            if (sale == null || sale.OrderDetails == null || !sale.OrderDetails.Any())
            {
                return null;
            }
            return new OrderResponseDTO
             (
                 Id: sale.Id,
                 TotalPrice: sale.OrderDetails.Sum(d => d.Quantity * d.Product.Price),
                 SaleDate: sale.OrderDate,
                 ProductId: sale.OrderDetails.First().ProductId,
                 ProductName: sale.OrderDetails.First().Product.Name
             );
        }
        public async Task<IEnumerable<OrderResponseDTO>> GetSalesAsync()
        {
            var salesQueryable = await _orderRepository.GetAllAsync();
            var saleResponse = salesQueryable.Select(s => new OrderResponseDTO
            (
                s.Id,
                s.OrderDetails.Sum(d => d.Quantity * d.Product.Price),
                s.OrderDate,
                s.OrderDetails.First().ProductId,
                s.OrderDetails.First().Product.Name

            ));

            // here the query is not executed yet just prepared to be executed later on 
            return await saleResponse.ToListAsync(); // here the query is executed
        }
        public async Task SoftDeleteSaleAsync(int id)
        {
            await _orderRepository.SoftDeleteAsync(id);

        }
        public async Task<IEnumerable<OrderResponseDTO>> GetSalesPerCustomerAsync(int id)
        {
            var salesQueryable = await _orderRepository.GetAllAsync();
            /*            var sales = await salesQueryable
                  .Where(s => s.CustomerId == id)
                  .Include(s => s.OrderDetails)
                  .ThenInclude(d => d.Product)
                  .Select(s => new OrderResponseDTO(
                      s.Id,
                      s.OrderDetails.Sum(d => d.Quantity * d.Product.Price),
                      s.OrderDate,
                      s.OrderDetails.FirstOrDefault().ProductId,
                      s.OrderDetails.FirstOrDefault().Product.Name
                  ))
                  .ToListAsync();*/
            var sales = from s in salesQueryable
                        where s.CustomerId == id
                        select new OrderResponseDTO
                            (
                                s.Id,
                                s.OrderDetails.Sum(d => d.Quantity * d.Product.Price),
                                s.OrderDate,
                                s.OrderDetails.FirstOrDefault().ProductId,
                                s.OrderDetails.FirstOrDefault().Product.Name
                            );
            return sales;
        }
        // difference between Ienumerable and IQueryable is that IEnumerable is in-memory data and IQueryable is a query that is not executed yet
        // what does in-memory mean?
        // in-memory means that the data is stored in the memory of the application, so it is not stored in the database
        public async Task UpdateSaleAsync(int id, OrderPostDTOController orderPostDTOController)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"Order with the id ${id} is not available");
            }

            foreach (var detail in orderPostDTOController.OrderDetailsWithProductRemaingStock)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.RemainingStock,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                order.OrderDetails.Add(orderDetail);
            }
            order.ModifiedDate = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);
        }
        /*public async Task<IEnumerable<Order>> GetSalesPerDateAsync(DateTime date)
        {
            var utcDate = date.ToUniversalTime().Date;
            var nextDate = utcDate.AddDays(1);

            var sales = await _orderRepository.FindAsync(s => s.SaleDate >= utcDate && s.SaleDate < nextDate);

            return sales;
        }
        public async Task<IEnumerable<Order>> GetSalesPerMonthAsync(int year, int month)
        {
            return await _orderRepository.FindAsync(s => s.SaleDate.Year == year && s.SaleDate.Month == month);
        }
        public async Task<IEnumerable<Order>> GetSalesPerWeekAsync(DateTime startDate)
        {
            var utcStartDate = startDate.ToUniversalTime();
            var utcEndDate = utcStartDate.AddDays(7);
            return await _orderRepository.FindAsync(s => s.SaleDate >= utcStartDate && s.SaleDate < utcEndDate);
        }*/
    }
}
