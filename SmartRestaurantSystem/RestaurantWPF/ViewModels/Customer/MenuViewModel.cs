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

namespace RestaurantWPF.ViewModels.Customer
{
    public class MenuViewModel : ObservableObject
    {
        private readonly IFoodService _foodService;
        private readonly ICategoryService _categoryService;
        private List<Food> _allFoods = new();
        private readonly IOrderService _orderService = new OrderService();
        private readonly IOrderDetailService _orderDetailService = new OrderDetailService();


        public ObservableCollection<Food> Foods { get; } = new();
        public ObservableCollection<Category> Categories { get; } = new();

        // 🛒 Giỏ hàng
        public ObservableCollection<CartItem> CartItems { get; } = new();
        private bool _isCartVisible;
        public bool IsCartVisible
        {
            get => _isCartVisible;
            set { _isCartVisible = value; OnPropertyChanged(); }
        }
        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                SelectedCategoryId = value?.CategoryId;
                OnPropertyChanged();
            }
        }

        public int CartCount => CartItems.Sum(i => i.Quantity);
        public decimal TotalPrice => CartItems.Sum(i => i.Price * i.Quantity);

        public int SelectedRating { get; set; }

        public ICommand RefreshCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand OpenCartDetailCommand { get; }

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveItemCommand { get; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private int? _selectedCategoryId;
        public int? SelectedCategoryId
        {
            get => _selectedCategoryId;
            set { _selectedCategoryId = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public MenuViewModel()
        {
            _foodService = new FoodService();
            _categoryService = new CategoryService();

            RefreshCommand = new RelayCommand(_ => ResetAndReload());
            AddToCartCommand = new RelayCommand(AddToCart);
            OpenCartDetailCommand = new RelayCommand(_ => OpenCartDetail());


            IncreaseQuantityCommand = new RelayCommand(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand(DecreaseQuantity);
            RemoveItemCommand = new RelayCommand(RemoveItem);
            LoadData();
        }

        private void IncreaseQuantity(object parameter)
        {
            if (parameter is CartItem item)
            {
                item.Quantity++;
                OnPropertyChanged(nameof(CartCount));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        private void DecreaseQuantity(object parameter)
        {
            if (parameter is CartItem item && item.Quantity > 1)
            {
                item.Quantity--;
            }
            else if (parameter is CartItem i)
            {
                CartItems.Remove(i);
            }
            OnPropertyChanged(nameof(CartCount));
            OnPropertyChanged(nameof(TotalPrice));
        }

        private void RemoveItem(object parameter)
        {
            if (parameter is CartItem item)
            {
                CartItems.Remove(item);
                OnPropertyChanged(nameof(CartCount));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        private void LoadData()
        {
            Categories.Clear();
            foreach (var c in _categoryService.GetAll().OrderBy(c => c.Name))
                Categories.Add(c);

            _allFoods = _foodService.GetAll();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = _allFoods.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var kw = SearchText.Trim().ToLower();
                query = query.Where(f => (f.Name ?? "").ToLower().Contains(kw)
                                       || (f.Description ?? "").ToLower().Contains(kw));
            }

            if (SelectedCategoryId.HasValue && SelectedCategoryId.Value > 0)
                query = query.Where(f => f.CategoryId == SelectedCategoryId.Value);

            Foods.Clear();
            foreach (var f in query) Foods.Add(f);
        }

        private void AddToCart(object parameter)
        {
            if (parameter is Food food)
            {
                var existing = CartItems.FirstOrDefault(c => c.FoodId == food.FoodId);
                if (existing != null)
                {
                    existing.Quantity++;
                }
                else
                {
                    CartItems.Add(new CartItem
                    {
                        FoodId = food.FoodId,
                        Name = food.Name,
                        Price = food.Price,
                        Quantity = 1,
                        ImageUrl = NormalizeImagePath(food.ImageUrl)
                    });
                }

                OnPropertyChanged(nameof(CartCount));
                OnPropertyChanged(nameof(TotalPrice));
                if (!IsCartVisible) IsCartVisible = true;
            }
        }


        private string? NormalizeImagePath(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            // Nếu ảnh online, giữ nguyên
            if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return url;

            // Nếu là ảnh trong thư mục Assets thì thêm prefix WPF hiểu được
            return $"pack://siteoforigin:,,,/{url.TrimStart('/')}";
        }


        private void ResetAndReload()
        {
            SearchText = string.Empty;
            SelectedCategory = null;  
            SelectedCategoryId = null;
            LoadData();             
        }


        private void OpenCartDetail()
        {
            var detailWindow = new Views.Customer.Components.CartDetailWindow
            {
                DataContext = this
            };
            detailWindow.ShowDialog();
        }

        public void PlaceOrder()
        {
            if (!CartItems.Any())
            {
                MessageBox.Show("Giỏ hàng của bạn đang trống!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var order = new Order
            {
                CustomerId = UserSession.UserId,
                OrderTime = DateTime.Now,
                Status = 0,
                Discount = 0,
                Note = $"Đặt hàng qua app bởi khách: {UserSession.FullName ?? "Khách chưa đăng nhập"}"
            };

            _orderService.Add(order);

            foreach (var item in CartItems)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    FoodId = item.FoodId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                };
                _orderDetailService.Add(detail);
            }

            MessageBox.Show("✅ Đặt món thành công! Nhân viên sẽ xác nhận đơn hàng của bạn.",
                "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            CartItems.Clear();
            OnPropertyChanged(nameof(CartCount));
            OnPropertyChanged(nameof(TotalPrice));
            IsCartVisible = false;
        }


    }
}
