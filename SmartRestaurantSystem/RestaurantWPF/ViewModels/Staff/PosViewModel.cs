using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using RestaurantWPF.Session;
using RestaurantWPF.Views.Staff.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Staff
{
    public class PosViewModel : BaseViewModel
    {
        // --- Services ---
        private readonly IFoodService _foodService;
        private readonly ICategoryService _categoryService;
        private readonly ITableService _tableService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        // --- Data ---
        private List<Food> _allFoods;
        public ObservableCollection<Food> Foods { get; } = new();
        public ObservableCollection<Category> Categories { get; } = new();
        public ObservableCollection<Table> Tables { get; } = new();
        public ObservableCollection<User> Customers { get; } = new();
        public ObservableCollection<OrderDetailViewModel> CurrentOrderItems { get; } = new();

        private Order _currentOrder;
        public Order CurrentOrder
        {
            get => _currentOrder;
            set { _currentOrder = value; OnPropertyChanged(); }
        }

        // --- POS Settings ---
        private bool _isTakeaway = true;
        public bool IsTakeaway
        {
            get => _isTakeaway;
            set
            {
                if (_isTakeaway != value)
                {
                    _isTakeaway = value;
                    OnPropertyChanged();
                    ToggleLabel = value ? "Mang đi" : "Tại bàn";
                    SubmitButtonText = value ? "Thanh toán" : "Xác nhận";
                }
            }
        }


        public ICommand OpenTableSelectionCommand { get; }
        public string SelectedTableName => CurrentOrder?.Table?.Name ?? "Chưa chọn bàn";


        private string _toggleLabel = "Mang đi";
        public string ToggleLabel
        {
            get => _toggleLabel;
            set { _toggleLabel = value; OnPropertyChanged(); }
        }


        private string _submitButtonText = "Thanh toán";
        public string SubmitButtonText
        {
            get => _submitButtonText;
            set { _submitButtonText = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterFoods(); }
        }

        private int? _selectedCategoryId;
        public int? SelectedCategoryId
        {
            get => _selectedCategoryId;
            set { _selectedCategoryId = value; OnPropertyChanged(); FilterFoods(); }
        }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set { _totalAmount = value; OnPropertyChanged(); }
        }

        private string _selectedPaymentMethod = "Tiền mặt";
        public string SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set { _selectedPaymentMethod = value; OnPropertyChanged(); }
        }

        // --- Commands ---
        public ICommand LoadMenuCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand ClearCartCommand { get; }
        public ICommand SubmitOrderCommand { get; }


        // --- Constructor ---
        public PosViewModel()
        {
            _foodService = new FoodService();
            _categoryService = new CategoryService();
            _tableService = new TableService();
            _userService = new UserService();
            _orderService = new OrderService();
            _paymentService = new PaymentService();

            ResetOrder();

            LoadCategories();
            LoadFoods();
            LoadTables();
            LoadCustomers();

            LoadMenuCommand = new RelayCommand(_ => LoadFoods());
            AddToCartCommand = new RelayCommand(AddToCart);
            IncreaseQuantityCommand = new RelayCommand(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand(DecreaseQuantity);
            RemoveFromCartCommand = new RelayCommand(RemoveFromCart);
            ClearCartCommand = new RelayCommand(ClearCart);
            SubmitOrderCommand = new RelayCommand(_ => SubmitOrder());
            OpenTableSelectionCommand = new RelayCommand(_ => OpenTableDialog());
        }

        private void OpenTableDialog()
        {
            var availableTables = _tableService
    .GetAll()
    .Where(t => t.Status == 0) // Chỉ bàn trống
    .ToList();

            var dialog = new TableSelectionDialog(availableTables);

            if (dialog.ShowDialog() == true && dialog.SelectedTable != null)
            {
                var selected = dialog.SelectedTable;

                // Gán lại cho Order.Table bằng bản Table thực
                CurrentOrder.TableId = selected.TableId;
                CurrentOrder.Table = new BusinessObjects.Models.Table
                {
                    TableId = selected.TableId,
                    Name = selected.Name,
                    Status = selected.Status
                };

                OnPropertyChanged(nameof(SelectedTableName));
            }
        }

        // --- Load data ---
        private void LoadFoods()
        {
            _allFoods = _foodService.GetAll().Where(f => f.IsAvailable == true).ToList();
            FilterFoods();
        }

        private void LoadCategories()
        {
            Categories.Clear();
            Categories.Add(new Category { CategoryId = 0, Name = "Tất cả Phân loại" });
            foreach (var category in _categoryService.GetAll())
                Categories.Add(category);
            SelectedCategoryId = 0;
        }

        private void LoadTables()
        {
            Tables.Clear();
            foreach (var t in _tableService.GetAll().Where(t => t.Status == 0))
                Tables.Add(t);
        }

        private void LoadCustomers()
        {
            Customers.Clear();
            foreach (var c in _userService.GetAll().Where(u => u.RoleId == 3))
                Customers.Add(c);
        }

        // --- Filtering ---
        private void FilterFoods()
        {
            if (_allFoods == null) return;

            Foods.Clear();
            var filtered = _allFoods.AsEnumerable();

            if (SelectedCategoryId.HasValue && SelectedCategoryId.Value > 0)
                filtered = filtered.Where(f => f.CategoryId == SelectedCategoryId.Value);

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string text = SearchText.ToLower().Trim();
                filtered = filtered.Where(f => f.Name != null && f.Name.ToLower().Contains(text));
            }

            foreach (var food in filtered)
                Foods.Add(food);
        }

        // --- Cart ---
        private void AddToCart(object obj)
        {
            if (obj is not Food food) return;

            var existing = CurrentOrderItems.FirstOrDefault(d => d.Food.FoodId == food.FoodId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                var model = new OrderDetail
                {
                    FoodId = food.FoodId,
                    Food = food,
                    Quantity = 1,
                    UnitPrice = food.Price
                };
                CurrentOrderItems.Add(new OrderDetailViewModel(model));
            }
            CalculateTotal();
        }

        private void IncreaseQuantity(object obj)
        {
            if (obj is OrderDetailViewModel item)
            {
                item.Quantity++;
                CalculateTotal();
            }
        }

        private void DecreaseQuantity(object obj)
        {
            if (obj is OrderDetailViewModel item)
            {
                if (item.Quantity > 1)
                    item.Quantity--;
                else
                    CurrentOrderItems.Remove(item);
                CalculateTotal();
            }
        }

        private void RemoveFromCart(object obj)
        {
            if (obj is OrderDetailViewModel item)
            {
                CurrentOrderItems.Remove(item);
                CalculateTotal();
            }
        }

        private void ClearCart(object obj)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                ResetOrder();
        }

        private void ResetOrder()
        {
            CurrentOrder = new Order
            {
                TableId = null,     // ✅ reset bàn
                Table = null,       // ✅ reset bàn trong entity
                CustomerId = null,  // ✅ reset khách nếu có
                Customer = null
            };

            CurrentOrderItems.Clear();
            TotalAmount = 0;
            SelectedPaymentMethod = "Tiền mặt";

            OnPropertyChanged(nameof(SelectedTableName)); // ✅ để UI cập nhật lại hiển thị “Chưa chọn bàn”
        }


        private void CalculateTotal()
        {
            TotalAmount = CurrentOrderItems.Sum(i => i.Quantity * i.UnitPrice);
        }

        // --- Submit logic ---
        private void SubmitOrder()
        {
            if (CurrentOrderItems.Count == 0)
            {
                MessageBox.Show("Chưa có món nào trong đơn hàng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentOrder.OrderTime = DateTime.Now;
            CurrentOrder.Discount = 0;
            CurrentOrder.StaffId = UserSession.UserId;
            CurrentOrder.OrderDetails = CurrentOrderItems.Select(i => new OrderDetail
            {
                FoodId = i.Food.FoodId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            if (IsTakeaway)
            {
                CurrentOrder.Type = OrderType.Takeaway;
                CurrentOrder.Status = 3; // Paid
                _orderService.Add(CurrentOrder);

                var payment = new Payment
                {
                    OrderId = CurrentOrder.OrderId,
                    PaidAmount = TotalAmount,
                    Method = ConvertPaymentMethod(SelectedPaymentMethod),
                    PaidAt = DateTime.Now
                };
                _paymentService.Add(payment);

                MessageBox.Show("Đã thanh toán đơn mang đi!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CurrentOrder.Type = OrderType.DineIn;
                CurrentOrder.Status = 1; // Serving
                _orderService.Add(CurrentOrder);

                // ✅ Cập nhật trạng thái bàn sang "Đang sử dụng"
                if (CurrentOrder.TableId.HasValue)
                {
                    var table = _tableService.GetById(CurrentOrder.TableId.Value);
                    if (table != null)
                    {
                        table.Status = 1; // Đang sử dụng
                        _tableService.Update(table);
                    }
                }

                MessageBox.Show("Đã tạo đơn ăn tại bàn!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            ResetOrder();
        }

        private byte ConvertPaymentMethod(string method)
        {
            return method switch
            {
                "Tiền mặt" => 0,
                "Thẻ" => 1,
                "Ví điện tử" => 2,
                _ => 0
            };
        }
    }
}
