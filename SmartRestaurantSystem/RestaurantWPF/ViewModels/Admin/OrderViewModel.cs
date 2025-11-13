using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using RestaurantWPF.Views.Admin.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Admin
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly IOrderService _orderService;
        public ObservableCollection<Order> Orders { get; set; }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set { _selectedOrder = value; OnPropertyChanged(); }
        }

        private List<Order> _allOrders;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter(); // gọi lọc lại mỗi khi nhập text
            }
        }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public ICommand ApplyFilterCommand { get; }
        public ICommand ClearFilterCommand { get; }



        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public OrderViewModel()
        {
            _orderService = new OrderService();
            Orders = new ObservableCollection<Order>();

            LoadCommand = new RelayCommand(_ => LoadOrders());
            AddCommand = new RelayCommand(_ => AddOrder());
            EditCommand = new RelayCommand(_ => EditOrder(), _ => SelectedOrder != null);
            DeleteCommand = new RelayCommand(_ => DeleteOrder(), _ => SelectedOrder != null);
            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());

            LoadOrders();
        }

        private void ApplyFilter()
        {
            if (_allOrders == null) return;

            var query = _allOrders.AsEnumerable();

            // 🔎 Search
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string kw = SearchText.ToLower();

                query = query.Where(o =>
                       o.OrderId.ToString().Contains(kw)
                    || (o.Table?.Name ?? "").ToLower().Contains(kw)
                    || (o.Customer?.FullName ?? "").ToLower().Contains(kw)
                    || (o.Staff?.FullName ?? "").ToLower().Contains(kw)
                    || (o.Note ?? "").ToLower().Contains(kw)
                    || o.Status.ToString().ToLower().Contains(kw)
                    || o.OrderTime.ToString("dd/MM/yyyy").Contains(kw)
                );
            }

            // 📅 From date
            if (FromDate.HasValue)
                query = query.Where(o => o.OrderTime >= FromDate.Value);

            // 📅 To date (đến cuối ngày)
            if (ToDate.HasValue)
                query = query.Where(o => o.OrderTime <= ToDate.Value.AddDays(1).AddSeconds(-1));

            // Đổ ra UI
            Orders.Clear();
            foreach (var o in query)
                Orders.Add(o);
        }



        private void ClearFilter()
        {
            SearchText = "";
            FromDate = null;
            ToDate = null;

            OnPropertyChanged(nameof(SearchText));
            OnPropertyChanged(nameof(FromDate));
            OnPropertyChanged(nameof(ToDate));

            ApplyFilter();
        }


        //private void ApplyFilter()
        //{
        //    if (_allOrders == null) return;

        //    Orders.Clear();

        //    var filtered = string.IsNullOrWhiteSpace(SearchText)
        //        ? _allOrders
        //        : _allOrders.Where(o =>
        //            o.OrderId.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
        //            (o.Table != null && o.Table.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
        //            (o.Customer != null && o.Customer.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
        //            (o.Staff != null && o.Staff.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
        //            (o.Note != null && o.Note.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
        //            (o.Status.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
        //            (o.OrderTime.ToString("dd/MM/yyyy").Contains(SearchText))
        //        ).ToList();

        //    foreach (var order in filtered)
        //        Orders.Add(order);
        //}



        private void LoadOrders()
        {
            _allOrders = _orderService.GetAll();
            ApplyFilter(); // hiển thị đầy đủ ban đầu
        }

        private void AddOrder()
        {
            var dialog = new Views.Admin.Dialogs.OrderDialog();
            if (dialog.ShowDialog() == true)
            {
                var newOrder = dialog.ViewModel.CurrentOrder;
                _orderService.Add(newOrder);
                LoadOrders();
            }
        }

        private void EditOrder()
        {
            if (SelectedOrder == null) return;

            var dialog = new Views.Admin.Dialogs.OrderDialog(SelectedOrder);
            if (dialog.ShowDialog() == true)
            {
                var updated = dialog.ViewModel.CurrentOrder;
                _orderService.Update(updated);
                LoadOrders();
            }
        }



        private void DeleteOrder()
        {
            if (SelectedOrder == null) return;
            if (MessageBox.Show("Delete this order?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _orderService.Delete(SelectedOrder.OrderId);
                LoadOrders();
            }
        }
    }
}
