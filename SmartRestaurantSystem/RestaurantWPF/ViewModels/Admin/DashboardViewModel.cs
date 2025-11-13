using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.DataContext;
//using LiveChartsCore;
//using LiveChartsCore.SkiaSharpView;
//using LiveChartsCore.SkiaSharpView.Painting;
//using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace RestaurantWPF.ViewModels.Admin
{
    public class DashboardViewModel : ObservableObject
    {
        private readonly IDashboardService _dashboardService;

        // 🧾 Các thuộc tính hiển thị thông tin tổng quan
        public int TotalFoods { get; set; }
        public int OrdersToday { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal RevenueToday { get; set; }

        public ObservableCollection<OrderSummary> RecentOrders { get; set; }
        public ObservableCollection<FoodStat> TopFoods { get; set; }

        // 📊 Các thuộc tính dành cho biểu đồ
        //public ISeries[] RevenueSeries { get; set; }
        //public Axis[] RevenueXAxes { get; set; }
        //public Axis[] RevenueYAxes { get; set; }

        //public ISeries[] TopFoodsSeries { get; set; }

        public DashboardViewModel()
        {
            _dashboardService = new DashboardService();
            LoadDashboard();
            //LoadCharts();
        }

        // --------------------------------------------------
        // 🧮 Load dữ liệu tổng quan dashboard
        // --------------------------------------------------
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

        // --------------------------------------------------
        // 📈 Load dữ liệu biểu đồ
        // --------------------------------------------------
        //private void LoadCharts()
        //{
        //    // 🧾 1️⃣ Doanh thu 7 ngày gần nhất
        //    var dailyRevenue = _dashboardService.GetRevenueByLast7Days();

        //    RevenueSeries = new ISeries[]
        //    {
        //        new ColumnSeries<decimal>
        //        {
        //            Values = dailyRevenue.Select(x => x.Total).ToArray(),
        //            Name = "Doanh thu (VNĐ)",
        //            Fill = new SolidColorPaint(SKColors.DeepSkyBlue),
        //            DataLabelsPaint = new SolidColorPaint(SKColors.DimGray),
        //            DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
        //            DataLabelsFormatter = point => point.Coordinate.PrimaryValue.ToString("N0")
        //        }
        //    };

        //    RevenueXAxes = new Axis[]
        //    {
        //        new Axis
        //        {
        //            Labels = dailyRevenue.Select(x => x.Date.ToString("dd/MM")).ToArray(),
        //            LabelsRotation = 0,
        //            TextSize = 12
        //        }
        //    };

        //    RevenueYAxes = new Axis[]
        //    {
        //        new Axis
        //        {
        //            Labeler = value => value.ToString("N0") + " đ",
        //            TextSize = 11
        //        }
        //    };

        //    var topFoods = _dashboardService.GetTopSellingFoods(5);
        //    var colorPalette = new[]
        //    {
        //        "#FF7A00", "#42A5F5", "#66BB6A", "#AB47BC", "#EF5350"
        //    };

        //    TopFoodsSeries = topFoods.Select(f => new PieSeries<decimal>
        //    {
        //        Name = f.FoodName,
        //        Values = new[] { (decimal)f.QuantitySold },
        //        DataLabelsSize = 14,
        //        DataLabelsPaint = new SolidColorPaint(SKColors.Black),
        //        DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue:N0} ({point.Context.Series.Name})",
        //        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle // ✅ đúng chỗ
        //    }).ToArray();


            // 🔄 Notify UI cập nhật
            //OnPropertyChanged(nameof(RevenueSeries));
            //OnPropertyChanged(nameof(RevenueXAxes));
            //OnPropertyChanged(nameof(RevenueYAxes));
            //OnPropertyChanged(nameof(TopFoodsSeries));
        }
    }

