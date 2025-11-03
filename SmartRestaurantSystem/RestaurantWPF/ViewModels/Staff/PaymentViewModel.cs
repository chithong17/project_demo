using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Staff
{
    public class PaymentViewModel : ObservableObject
    {
        private readonly IOrderService _orderService = new OrderService();
        private readonly IPaymentService _paymentService = new PaymentService();
        private readonly ITableService _tableService = new TableService();
        public ObservableCollection<Payment> PaymentsHistory { get; } = new();


        private void LoadPaymentsHistory()
        {
            PaymentsHistory.Clear();
            var payments = _paymentService.GetAll()
                .OrderByDescending(p => p.PaidAt)
                .ToList();

            foreach (var p in payments)
                PaymentsHistory.Add(p);
        }

        public ObservableCollection<Order> PendingOrders { get; } = new();

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
                IsOrderSelected = _selectedOrder != null;
                if (_selectedOrder != null)
                    CalculateSuggestedAmount();
            }
        }

        private bool _isOrderSelected;
        public bool IsOrderSelected
        {
            get => _isOrderSelected;
            set { _isOrderSelected = value; OnPropertyChanged(); }
        }

        private decimal _paidAmount;
        public decimal PaidAmount
        {
            get => _paidAmount;
            set { _paidAmount = value; OnPropertyChanged(); }
        }

        private byte _selectedMethod;
        public byte SelectedMethod
        {
            get => _selectedMethod;
            set { _selectedMethod = value; OnPropertyChanged(); }
        }

        private string _note;
        public string Note
        {
            get => _note;
            set { _note = value; OnPropertyChanged(); }
        }

        public ICommand ConfirmPaymentCommand { get; }

        public PaymentViewModel()
        {
            ConfirmPaymentCommand = new RelayCommand(ConfirmPayment);
            LoadPaymentsHistory();
            LoadPendingOrders();
        }

        private void LoadPendingOrders()
        {
            PendingOrders.Clear();
            var orders = _orderService.GetAll()
                .Where(o => o.Status == 2 || (o.Status == 3 && o.Payment == null))
                .OrderByDescending(o => o.OrderTime)
                .ToList();

            foreach (var o in orders)
                PendingOrders.Add(o);
        }

        private void CalculateSuggestedAmount()
        {
            if (SelectedOrder?.OrderDetails == null || !SelectedOrder.OrderDetails.Any())
            {
                PaidAmount = 0;
                return;
            }

            PaidAmount = SelectedOrder.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
        }

        private void ConfirmPayment(object obj)
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (PaidAmount <= 0)
            {
                MessageBox.Show("Số tiền thanh toán không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var payment = new Payment
            {
                OrderId = SelectedOrder.OrderId,
                Method = SelectedMethod,
                PaidAmount = PaidAmount,
                PaidAt = DateTime.Now,
                Note = Note
            };

            try
            {
                _paymentService.Add(payment);

                // Cập nhật trạng thái order và bàn
                SelectedOrder.Status = 3; // Completed
                _orderService.Update(SelectedOrder);

                if (SelectedOrder.TableId.HasValue)
                {
                    var table = _tableService.GetById(SelectedOrder.TableId.Value);
                    if (table != null)
                    {
                        table.Status = 0; // Trống
                        _tableService.Update(table);
                    }
                }

                MessageBox.Show("Thanh toán thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadPendingOrders();
                IsOrderSelected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý thanh toán: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
