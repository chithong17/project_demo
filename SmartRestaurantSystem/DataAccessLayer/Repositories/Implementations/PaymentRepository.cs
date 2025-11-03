using BusinessObjects.Models;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.Interfaces;
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

        public List<Payment> GetAll() => _context.Payments.ToList();

        public Payment GetByOrderId(int orderId) => _context.Payments.FirstOrDefault(p => p.OrderId == orderId);

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }
    }
}
