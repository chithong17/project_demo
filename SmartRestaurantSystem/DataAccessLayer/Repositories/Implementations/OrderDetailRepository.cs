using BusinessObjects.Models;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Implementations
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly SmartRestaurantDbContext _context = new();

        public List<OrderDetail> GetByOrderId(int orderId) =>
            _context.OrderDetails.Where(d => d.OrderId == orderId).ToList();

        public void Add(OrderDetail detail)
        {
            _context.OrderDetails.Add(detail);
            _context.SaveChanges();
        }

        public void Delete(int orderId, int foodId)
        {
            var d = _context.OrderDetails.FirstOrDefault(x => x.OrderId == orderId && x.FoodId == foodId);
            if (d != null)
            {
                _context.OrderDetails.Remove(d);
                _context.SaveChanges();
            }
        }

        public List<OrderDetail> GetAll()
        {
            return _context.OrderDetails
                .Include(d => d.Food)
                .Include(d => d.Order)
                .AsNoTracking()
                .ToList();
        }
    }
}
