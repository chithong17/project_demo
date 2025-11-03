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

            LoadOrders();
        }

        private void LoadOrders()
        {
            Orders.Clear();
            var list = _orderService.GetAll();
            foreach (var item in list)
                Orders.Add(item);
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
