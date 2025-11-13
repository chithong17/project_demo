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
    public class ReservationViewModel : ObservableObject
    {
        private readonly IReservationService _reservationService = new ReservationService();
        private readonly IOrderService _orderService = new OrderService();
        private readonly ITableService _tableService = new TableService();

        public ObservableCollection<Reservation> Reservations { get; } = new();
        public ObservableCollection<Table> AvailableTables { get; } = new();

        private Reservation _selectedReservation;
        public Reservation SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
                IsReservationSelected = _selectedReservation != null;
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilters(); // ✅ tự động lọc khi text thay đổi
            }
        }
        private List<Reservation> _allReservations = new();

        private DateTime? _filterDate;
        public DateTime? FilterDate
        {
            get => _filterDate;
            set
            {
                _filterDate = value;
                OnPropertyChanged();
                ApplyFilters(); // ✅ tự động lọc khi chọn ngày
            }
        }


        private Table _selectedTable;
        public Table SelectedTable
        {
            get => _selectedTable;
            set { _selectedTable = value; OnPropertyChanged(); }
        }

        private bool _isReservationSelected;
        public bool IsReservationSelected
        {
            get => _isReservationSelected;
            set { _isReservationSelected = value; OnPropertyChanged(); }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ConvertToOrderCommand { get; }
        public ICommand AssignTableCommand { get; }

        public ICommand RefreshCommand { get; }

        public ReservationViewModel()
        {
            ConfirmCommand = new RelayCommand(ConfirmReservation);
            CancelCommand = new RelayCommand(CancelReservation);
            ConvertToOrderCommand = new RelayCommand(ConvertToOrder);
            AssignTableCommand = new RelayCommand(AssignTable);
            RefreshCommand = new RelayCommand(_ => { SearchText = ""; FilterDate = null; LoadReservations(); });
            LoadReservations();
        }

        private void LoadReservations()
        {
            Reservations.Clear();
            _allReservations = _reservationService.GetAll()
                .OrderByDescending(r => r.StartTime)
                .ToList();

            ApplyFilters(); // ✅ áp dụng filter sau khi load
            LoadAvailableTables();
        }
        private void ApplyFilters()
        {
            var filtered = _allReservations.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(r =>
                    (!string.IsNullOrEmpty(r.CustomerName) && r.CustomerName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(r.Phone) && r.Phone.Contains(SearchText))
                );
            }

            if (FilterDate.HasValue)
            {
                filtered = filtered.Where(r =>
                    r.StartTime.Date == FilterDate.Value.Date
                );
            }

            Reservations.Clear();
            foreach (var r in filtered)
                Reservations.Add(r);
        }

        private void LoadAvailableTables()
        {
            AvailableTables.Clear();
            var list = _tableService.GetAvailable();
            foreach (var t in list)
                AvailableTables.Add(t);
        }

        private void ConfirmReservation(object obj)
        {
            if (obj is not Reservation res) return;
            if (res.Status != 0)
            {
                MessageBox.Show("Chỉ có thể xác nhận các đặt bàn đang chờ!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            res.Status = 1; // Chờ gán bàn
            _reservationService.Update(res);
            LoadReservations();
            MessageBox.Show("Đã xác nhận đặt bàn. Hãy chọn bàn để gán!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AssignTable(object obj)
        {
            if (SelectedReservation == null)
            {
                MessageBox.Show("Vui lòng chọn đặt bàn để gán.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Lưu lại ID trước khi reload
                var currentId = SelectedReservation?.ReservationId;

                // Cập nhật dữ liệu
                SelectedReservation.TableId = SelectedTable.TableId;
                SelectedReservation.Status = 2; // Đã gán bàn
                SelectedTable.Status = 2;       // Reserved

                _reservationService.Update(SelectedReservation);
                _tableService.Update(SelectedTable);

                MessageBox.Show($"Đã gán bàn '{SelectedTable.Name}' cho đặt bàn #{SelectedReservation.ReservationId}.",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadReservations();

                // Giữ lại đơn vừa thao tác
                if (currentId != null)
                    SelectedReservation = Reservations.FirstOrDefault(r => r.ReservationId == currentId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelReservation(object obj)
        {
            if (obj is not Reservation res) return;

            // Nếu đã gán bàn thì phải trả bàn về trạng thái trống
            if (res.TableId != null)
            {
                var table = _tableService.GetById(res.TableId.Value);
                if (table != null)
                {
                    table.Status = 0;
                    _tableService.Update(table);
                }
            }

            res.Status = 4; // Hủy
            _reservationService.Update(res);
            LoadReservations();

            MessageBox.Show("Đã hủy đặt bàn và trả bàn về trạng thái trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ConvertToOrder(object obj)
        {
            if (obj is not Reservation res) return;

            if (res.Status != 2 || res.TableId == null)
            {
                MessageBox.Show("Chỉ có thể chuyển các đặt bàn đã gán bàn!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var currentId = res.ReservationId;
                var table = _tableService.GetById(res.TableId.Value);
                if (table == null)
                {
                    MessageBox.Show("Không tìm thấy bàn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newOrder = new Order
                {
                    CustomerId = res.CustomerId,
                    TableId = table.TableId,
                    OrderTime = DateTime.Now,
                    Status = 1 // Pending
                };

                _orderService.Add(newOrder);
                res.Status = 3; // Đã đến
                _reservationService.Update(res);
                table.Status = 1; // Occupied
                _tableService.Update(table);

                LoadReservations();

                SelectedReservation = Reservations.FirstOrDefault(r => r.ReservationId == currentId);

                MessageBox.Show($"Đã chuyển đặt bàn #{res.ReservationId} thành Order #{newOrder.OrderId} (Bàn {table.Name}).",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
