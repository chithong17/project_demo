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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo = new OrderRepository();
        private readonly IPaymentRepository _paymentRepo = new PaymentRepository();

        public List<Order> GetAll() => _orderRepo.GetAll();
        public Order GetById(int id) => _orderRepo.GetById(id);
        public void Add(Order o) => _orderRepo.Add(o);
        public void Update(Order o) => _orderRepo.Update(o);
        public void Delete(int id) => _orderRepo.Delete(id);

        public decimal GetTodayRevenue()
        {
            var today = DateTime.Now.Date;
            return _paymentRepo.GetAll()
                .Where(p => p.PaidAt.Date == today)
                .Sum(p => p.PaidAmount);
        }


        public void AddFoodToOrder(int orderId, int foodId, int quantity)
        {
            var order = _orderRepo.GetById(orderId);
            if (order == null)
                throw new Exception("Không tìm thấy đơn hàng.");

            var food = new FoodRepository().GetById(foodId);
            if (food == null)
                throw new Exception("Không tìm thấy món ăn.");

            var detail = new OrderDetail
            {
                OrderId = orderId,
                FoodId = foodId,
                Quantity = quantity,
                UnitPrice = food.Price
            };

            _orderRepo.AddOrderDetail(detail);
        }

    }
}
