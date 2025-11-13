using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using RestaurantWPF.Session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RestaurantWPF.ViewModels.Customer
{
    public class FeedbackViewModel : ObservableObject
    {
        private readonly IFeedbackService _feedbackService = new FeedbackService();
        private readonly IOrderService _orderService = new OrderService();

        public ObservableCollection<Food> CompletedFoods { get; } = new();
        public ObservableCollection<Feedback> FeedbackHistory { get; } = new();

        // 🥘 Món ăn được chọn
        private Food _selectedFood;
        public Food SelectedFood
        {
            get => _selectedFood;
            set { _selectedFood = value; OnPropertyChanged(); }
        }

        // ⭐ Điểm đánh giá được chọn
        private int _selectedRating;
        public int SelectedRating
        {
            get => _selectedRating;
            set { _selectedRating = value; OnPropertyChanged(); }
        }

        // 💬 Nhận xét
        private string _comment;
        public string Comment
        {
            get => _comment;
            set { _comment = value; OnPropertyChanged(); }
        }

        // ⭐ Danh sách các sao (1–5)
        public List<int> Ratings { get; } = Enumerable.Range(1, 5).ToList();

        public ICommand SubmitFeedbackCommand { get; }
        public ICommand SelectRatingCommand { get; }

        public FeedbackViewModel()
        {
            SubmitFeedbackCommand = new RelayCommand(_ => SubmitFeedback());
            SelectRatingCommand = new RelayCommand(param =>
            {
                if (param is int val)
                    SelectedRating = val;
            });

            LoadCompletedFoods();
            LoadFeedbackHistory();
        }

        // 🥗 Lấy danh sách món ăn trong các đơn hàng hoàn tất
        private void LoadCompletedFoods()
        {
            CompletedFoods.Clear();

            var completedOrders = _orderService.GetAll()
                .Where(o => o.CustomerId == UserSession.UserId && o.Status == 3)
                .ToList();

            var allFoods = completedOrders
                .SelectMany(o => o.OrderDetails.Select(d => d.Food))
                .Distinct()
                .ToList();


            foreach (var f in allFoods)
                CompletedFoods.Add(f);


        }

        // 🕒 Lịch sử đánh giá trước đó
        private void LoadFeedbackHistory()
        {
            FeedbackHistory.Clear();

            var list = _feedbackService.GetAll()
                .Where(f => f.CustomerId == UserSession.UserId)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();

            foreach (var fb in list)
            {
                fb.RatingStars = Enumerable.Range(1, 5)
                    .Select(i => i <= fb.Rating ? "Gold" : "LightGray") // chỉ string
                    .ToList();

                FeedbackHistory.Add(fb);
            }
        }

        // 💌 Gửi đánh giá
        private void SubmitFeedback()
        {
            if (SelectedFood == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn để đánh giá!", "Thiếu thông tin",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedRating <= 0)
            {
                MessageBox.Show("Vui lòng chọn số sao hợp lệ!", "Thiếu thông tin",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 🔍 Tìm order hoàn tất chứa món này
            var relatedOrder = _orderService.GetAll()
                .FirstOrDefault(o =>
                    o.CustomerId == UserSession.UserId &&
                    o.Status == 3 &&
                    o.OrderDetails.Any(d => d.FoodId == SelectedFood.FoodId));

            if (relatedOrder == null)
            {
                MessageBox.Show("Không tìm thấy đơn hàng chứa món này!", "Lỗi dữ liệu",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var feedback = new Feedback
            {
                CustomerId = UserSession.UserId,
                FoodId = SelectedFood.FoodId,
                OrderId = relatedOrder.OrderId, // ✅ Gán đúng OrderId
                Rating = SelectedRating,
                Comment = Comment,
                CreatedAt = DateTime.Now
            };

            _feedbackService.Add(feedback);

            MessageBox.Show("Cảm ơn bạn đã đánh giá món ăn 💖", "Thành công",
                MessageBoxButton.OK, MessageBoxImage.Information);

            Comment = string.Empty;
            SelectedRating = 0;
            OnPropertyChanged(nameof(Comment));
            OnPropertyChanged(nameof(SelectedRating));

            LoadFeedbackHistory();
        }

    }
}
