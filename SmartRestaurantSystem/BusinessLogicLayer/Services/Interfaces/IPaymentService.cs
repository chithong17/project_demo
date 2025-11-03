using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IPaymentService
    {
        List<Payment> GetAll();
        Payment GetByOrderId(int orderId);
        void Add(Payment p);
    }
}
