using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.DataContext;
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

        public List<DailyRevenueDTO> GetRevenueByLast7Days()
        {
            using var context = new SmartRestaurantDbContext();
            var today = DateTime.Now.Date;

            var result = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-i))
                .OrderBy(x => x)
                .Select(date =>
                {
                    var paidRevenue = context.Payments
                        .Where(p => p.PaidAt.Date == date)
                        .Sum(p => (decimal?)p.PaidAmount) ?? 0;

                    if (paidRevenue == 0)
                    {
                        paidRevenue = context.Orders
                            .Where(o => o.OrderTime.Date == date)
                            .SelectMany(o => o.OrderDetails)
                            .Sum(od => (decimal?)(od.UnitPrice * od.Quantity)) ?? 0;
                    }

                    return new DailyRevenueDTO
                    {
                        Date = date,
                        Total = paidRevenue
                    };
                })
                .ToList();

            return result;
        }


        public List<TopFoodDTO> GetTopSellingFoods(int count)
        {
            using var context = new SmartRestaurantDbContext();
            return context.OrderDetails
                .GroupBy(od => od.Food.Name)
                .Select(g => new TopFoodDTO
                {
                    FoodName = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(count)
                .ToList();
        }

    }
}
