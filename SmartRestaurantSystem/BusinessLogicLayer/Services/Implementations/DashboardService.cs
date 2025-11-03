using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IFoodRepository _foodRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;

        public DashboardService()
        {
            _foodRepo = new FoodRepository();
            _orderRepo = new OrderRepository();
            _orderDetailRepo = new OrderDetailRepository();
        }

        public DashboardSummary GetDashboardSummary()
        {
            var today = DateTime.Today;
            var orders = _orderRepo.GetAll();
            var details = _orderDetailRepo.GetAll();

            // ⚙️ Tính doanh thu: từ Payment nếu có
            var summary = new DashboardSummary
            {
                TotalFoods = _foodRepo.GetAll().Count,

                OrdersToday = orders.Count(o => o.OrderTime.Date == today),

                ActiveCustomers = orders
                    .Where(o => o.OrderTime.Date == today && o.CustomerId.HasValue)
                    .Select(o => o.CustomerId.Value)
                    .Distinct()
                    .Count(),

                RevenueToday = orders
                    .Where(o => o.OrderTime.Date == today && o.Payment != null)
                    .Sum(o => (decimal?)o.Payment.PaidAmount) ?? 0
            };

            // 🧾 Đơn hàng gần nhất
            summary.RecentOrders = orders
                .OrderByDescending(o => o.OrderTime)
                .Take(5)
                .Select(o => new OrderSummary
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Customer?.FullName ?? "Guest",
                    TableName = o.Table?.Name ?? "N/A",
                    Total = o.Payment?.PaidAmount ?? 0,
                    Status = o.Status.ToString(),
                    CreatedAt = o.OrderTime
                })
                .ToList();

            // 🍽️ Top món ăn bán chạy
            summary.TopFoods = details
                .GroupBy(d => d.Food.Name)
                .Select(g => new FoodStat
                {
                    Name = g.Key,
                    OrdersCount = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(f => f.OrdersCount)
                .Take(5)
                .ToList();

            return summary;
        }
    }
}
