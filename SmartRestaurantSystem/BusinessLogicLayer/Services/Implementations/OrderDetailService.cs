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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _repo = new OrderDetailRepository();

        public List<OrderDetail> GetByOrderId(int orderId) => _repo.GetByOrderId(orderId);
        public void Add(OrderDetail d) => _repo.Add(d);
        public void Delete(int orderId, int foodId) => _repo.Delete(orderId, foodId);
    }
}
