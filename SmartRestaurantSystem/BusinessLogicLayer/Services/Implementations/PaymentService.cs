using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo = new PaymentRepository();

        public List<Payment> GetAll() => _repo.GetAll();
        public Payment GetByOrderId(int orderId) => _repo.GetByOrderId(orderId);
        public void Add(Payment p) => _repo.Add(p);
    }
}
