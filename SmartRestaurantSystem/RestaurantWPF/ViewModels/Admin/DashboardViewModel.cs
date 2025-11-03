using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.DataContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantWPF.ViewModels.Admin
{
    public class DashboardViewModel : ObservableObject
    {
        private readonly IDashboardService _dashboardService;

        // Các thuộc tính hiển thị
        public int TotalFoods { get; set; }
        public int OrdersToday { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal RevenueToday { get; set; }

        public ObservableCollection<OrderSummary> RecentOrders { get; set; }
        public ObservableCollection<FoodStat> TopFoods { get; set; }

        public DashboardViewModel()
        {
            _dashboardService = new DashboardService();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            var data = _dashboardService.GetDashboardSummary();

            TotalFoods = data.TotalFoods;
            OrdersToday = data.OrdersToday;
            ActiveCustomers = data.ActiveCustomers;
            RevenueToday = data.RevenueToday;

            RecentOrders = new ObservableCollection<OrderSummary>(data.RecentOrders);
            TopFoods = new ObservableCollection<FoodStat>(data.TopFoods);

            OnPropertyChanged(nameof(TotalFoods));
            OnPropertyChanged(nameof(OrdersToday));
            OnPropertyChanged(nameof(ActiveCustomers));
            OnPropertyChanged(nameof(RevenueToday));
            OnPropertyChanged(nameof(RecentOrders));
            OnPropertyChanged(nameof(TopFoods));
        }
    }
}

