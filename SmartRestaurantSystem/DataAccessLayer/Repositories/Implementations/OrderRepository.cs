using BusinessObjects.Models;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetAll()
        {
            using var context = new SmartRestaurantDbContext();
            var orders = context.Orders
                .Include(o => o.Table)
                .Include(o => o.Staff)
                .Include(o => o.Customer)
                .Include(o => o.Payment)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Food)
                .AsNoTracking()
                .OrderByDescending(o => o.OrderTime)
                .ToList();

            Console.WriteLine($"Loaded {orders.Count} orders");

            foreach (var order in orders)
            {
                Console.WriteLine($"Order {order.OrderId}: {order.OrderDetails?.Count ?? 0} món");
                foreach (var detail in order.OrderDetails ?? new List<OrderDetail>())
                    Console.WriteLine($"  - {detail.Food?.Name ?? "Food null"}");
            }

            return orders;
        }

        public Order GetById(int id)
        {
            using var context = new SmartRestaurantDbContext();
            return context.Orders
                .Include(o => o.Table)
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Food)
                .FirstOrDefault(o => o.OrderId == id);
        }

        public void Add(Order order)
        {
            using var context = new SmartRestaurantDbContext();
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public void Update(Order order)
        {
            using var context = new SmartRestaurantDbContext();
            context.Orders.Update(order);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = new SmartRestaurantDbContext();

            var order = context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Feedbacks)
                .Include(o => o.Payment)
                .FirstOrDefault(o => o.OrderId == id);

            if (order != null)
            {
                if (order.OrderDetails.Any())
                    context.OrderDetails.RemoveRange(order.OrderDetails);

                if (order.Feedbacks.Any())
                    context.Feedbacks.RemoveRange(order.Feedbacks);

                if (order.Payment != null)
                    context.Payments.Remove(order.Payment);

                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }

        public void AddOrderDetail(OrderDetail detail)
        {
            using var context = new SmartRestaurantDbContext();
            context.OrderDetails.Add(detail);
            context.SaveChanges();
        }


    }
}
