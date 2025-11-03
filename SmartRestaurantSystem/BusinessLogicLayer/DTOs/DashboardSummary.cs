using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class DashboardSummary
    {
        public int TotalFoods { get; set; }
        public int OrdersToday { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal RevenueToday { get; set; }

        public List<OrderSummary> RecentOrders { get; set; }

        public List<FoodStat> TopFoods { get; set; }
    }

    public class OrderSummary
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string TableName { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // 🔥 DTO món ăn bán chạy
    public class FoodStat
    {
        public string Name { get; set; }
        public int OrdersCount { get; set; }
        public decimal Revenue { get; set; }
    }
}
