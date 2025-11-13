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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly SmartRestaurantDbContext _context = new();

        public List<Payment> GetAll()
        {
            return _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .ToList();
        }


        public Payment GetByOrderId(int orderId) => _context.Payments.FirstOrDefault(p => p.OrderId == orderId);

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }
    }
}
