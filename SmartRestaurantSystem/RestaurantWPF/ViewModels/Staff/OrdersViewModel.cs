using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Staff
{
    public class OrdersViewModel : ObservableObject
    {
        private readonly IOrderService _orderService = new OrderService();
        private readonly IFoodService _foodService = new FoodService(); // ✅ dùng FoodService để load món

        // ===================== DANH SÁCH ORDER =====================
        public ObservableCollection<Order> OnsiteOrders { get; } = new();
        public ObservableCollection<Order> PreOrders { get; } = new();

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged();
                    IsOrderSelected = _selectedOrder != null;
                    if (_selectedOrder != null)
                    {
                        LoadFoods(); // ✅ khi chọn đơn, nạp danh sách món
                        Quantity = 1; // reset số lượng
                        SelectedFood = null;
                        UpdateProgress(); 
                    }
                }
            }
        }

        private bool _isOrderSelected;
        public bool IsOrderSelected
        {
            get => _isOrderSelected;
            set { _isOrderSelected = value; OnPropertyChanged(); }
        }

        // ===================== DANH SÁCH MÓN (để thêm vào order) =====================
        public ObservableCollection<Food> AvailableFoods { get; } = new();

        private Food _selectedFood;
        public Food SelectedFood
        {
            get => _selectedFood;
            set { _selectedFood = value; OnPropertyChanged(); }
        }

        private int _quantity = 1;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        // ===================== COMMANDS =====================
        public ICommand ServeCommand { get; }
        public ICommand CompleteCommand { get; }
        public ICommand ConfirmPreOrderCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddFoodCommand { get; } // ✅ thêm món
        public ObservableCollection<OrderStepViewModel> OrderSteps { get; set; }

        public OrdersViewModel()
        {
            ServeCommand = new RelayCommand(ServeOrder);
            CompleteCommand = new RelayCommand(CompleteOrder);
            ConfirmPreOrderCommand = new RelayCommand(ConfirmPreOrder);
            CancelCommand = new RelayCommand(CancelOrder);
            AddFoodCommand = new RelayCommand(AddFoodToOrder);
            OrderSteps = new ObservableCollection<OrderStepViewModel>
            {
                new("Đặt trước", 0),
                new("Xác nhận", 1),
                new("Phục vụ", 2),
                new("Hoàn tất", 3)
            };
            LoadOrders();
        }

        private void UpdateProgress()
        {
            if (SelectedOrder == null) return;

            foreach (var step in OrderSteps)
                step.IsCompleted = SelectedOrder.Status >= step.StepValue;
        }

        // ===================== LOAD DATA =====================
        private void LoadOrders()
        {
            OnsiteOrders.Clear();
            PreOrders.Clear();

            var allOrders = _orderService.GetAll()
                .OrderByDescending(o => o.OrderTime)
                .ToList();

            foreach (var order in allOrders)
            {
                if (order.TableId != null) OnsiteOrders.Add(order);
                else PreOrders.Add(order);
            }
        }

        private void LoadFoods()
        {
            AvailableFoods.Clear();
            // ⚠️ Entity Food của bạn có IsAvailable (bool), KHÔNG có Status
            foreach (var food in _foodService.GetAll().Where(f => f.IsAvailable))
                AvailableFoods.Add(food);
        }

        // ===================== THÊM MÓN VÀO ORDER (SỬA CHO KHỚP SERVICE) =====================
        private void AddFoodToOrder(object _)
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedFood == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Quantity <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // ✅ Gọi đúng chữ ký service hiện tại của bạn
                _orderService.AddFoodToOrder(SelectedOrder.OrderId, SelectedFood.FoodId, Quantity);

                MessageBox.Show(
                    $"Đã thêm {SelectedFood.Name} x{Quantity} vào đơn #{SelectedOrder.OrderId}.",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Vì GetAll() đang AsNoTracking, sau khi thêm nên refresh lại SelectedOrder từ DB:
                var refreshed = _orderService.GetById(SelectedOrder.OrderId);
                // Gán lại để UI nạp OrderDetails mới
                SelectedOrder = refreshed;

                // Nếu muốn giữ nguyên danh sách listview và chỉ refresh panel:
                // không cần gọi LoadOrders(); nhưng nếu cần đồng bộ list, uncomment:
                // LoadOrders();
                // RefreshSelection(refreshed.OrderId);

                // Reset lựa chọn thêm món
                Quantity = 1;
                SelectedFood = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm món: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===================== TRẠNG THÁI ORDER =====================
        private void ServeOrder(object obj)
        {
            if (obj is not Order order) return;

            if (order.Status != 1)
            {
                MessageBox.Show("Đơn này không ở trạng thái 'Chờ phục vụ'!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            order.Status = 2;
            _orderService.Update(order);
            MessageBox.Show("Đơn đã chuyển sang 'Đang phục vụ'.");
            LoadOrders();
            RefreshSelection(order.OrderId);
            UpdateProgress();
        }

        private void CompleteOrder(object obj)
        {
            if (obj is not Order order) return;
            if (order.Status != 2)
            {
                MessageBox.Show("Đơn chưa được phục vụ!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            order.Status = 3;
            _orderService.Update(order);
            MessageBox.Show("Đơn đã hoàn tất!");
            LoadOrders();
            RefreshSelection(order.OrderId);
            UpdateProgress();
        }

        private void ConfirmPreOrder(object obj)
        {
            if (obj is not Order order) return;
            if (order.Status != 0)
            {
                MessageBox.Show("Đơn này đã được xác nhận hoặc đang xử lý!");
                return;
            }

            order.Status = 1;
            _orderService.Update(order);
            MessageBox.Show("Đã xác nhận đơn đặt trước.");
            LoadOrders();
            RefreshSelection(order.OrderId);
            UpdateProgress();
        }

        private readonly ITableService _tableService = new TableService(); // ⚡ thêm dòng này ở đầu class

        private void CancelOrder(object obj)
        {
            if (obj is not Order order) return;

            // Xác nhận trước khi hủy
            if (MessageBox.Show("Bạn có chắc muốn hủy đơn này không?", "Xác nhận",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            // Đặt trạng thái đơn hàng = 4 (Hủy)
            order.Status = 4;
            _orderService.Update(order);

            // ✅ Nếu đơn có bàn, thì giải phóng bàn đó (status = 0)
            if (order.TableId.HasValue)
            {
                var table = _tableService.GetById(order.TableId.Value);
                if (table != null)
                {
                    table.Status = 0; // Trả bàn về "trống"
                    _tableService.Update(table);
                }
            }

            MessageBox.Show("Đơn đã được hủy và bàn đã được giải phóng.",
                            "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Refresh lại danh sách đơn
            LoadOrders();
            SelectedOrder = null;
            UpdateProgress();
        }


        private void RefreshSelection(int orderId)
        {
            var all = OnsiteOrders.Concat(PreOrders).ToList();
            SelectedOrder = all.FirstOrDefault(o => o.OrderId == orderId);
        }
    }
}
