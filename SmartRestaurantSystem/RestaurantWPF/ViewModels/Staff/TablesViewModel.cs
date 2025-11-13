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
    public class TablesViewModel : ObservableObject
    {
        private readonly ITableService _tableService = new TableService();

        public ObservableCollection<Table> Tables { get; } = new();
        private List<Table> _allTables;
        private Table _selectedTable;
        public Table SelectedTable
        {
            get => _selectedTable;
            set
            {
                if (_selectedTable != value)
                {
                    _selectedTable = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsTableSelected)); // ✅ thêm dòng này
                    IsTableSelected = _selectedTable != null;
                }
            }
        }

        private bool _isTableSelected;
        public bool IsTableSelected
        {
            get => _isTableSelected;
            set { _isTableSelected = value; OnPropertyChanged(); }
        }


        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }
        // Command
        public ICommand RefreshCommand { get; }
        public ICommand ChangeStatusCommand { get; }
        public ICommand ClearTableCommand { get; } 
        public ICommand AddTableCommand { get; }
        public TablesViewModel()
        {
            RefreshCommand = new RelayCommand(_ => LoadTables());
            ChangeStatusCommand = new RelayCommand(ChangeStatus);
            ClearTableCommand = new RelayCommand(ClearTable); 
            AddTableCommand = new RelayCommand(AddTable);
            LoadTables();
        }

        private void LoadTables()
        {
            Tables.Clear();
            _allTables = _tableService.GetAll()
                .Where(t => !t.IsDeleted) // ✅ tránh bàn bị xóa mềm
                .ToList();

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allTables == null) return;

            Tables.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allTables
                : _allTables.Where(t =>
                    (t.Name != null && t.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (t.Location != null && t.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();

            foreach (var t in filtered)
                Tables.Add(t);
        }

        private void ChangeStatus(object obj)
        {
            if (obj is not Table table) return;

            // Chu kỳ: 0 -> 1 -> 3 -> 0
            byte newStatus = table.Status switch
            {
                0 => 1, // Trống -> Có khách
                1 => 3, // Có khách -> Đang dọn
                3 => 0, // Đang dọn -> Trống
                _ => table.Status
            };

            if (newStatus == table.Status)
            {
                MessageBox.Show("Không thể thay đổi trạng thái của bàn này.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            table.Status = newStatus;
            _tableService.Update(table);

            MessageBox.Show($"Đã đổi trạng thái bàn '{table.Name}' thành {GetStatusText(newStatus)}",
                "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadTables();
            SelectedTable = Tables.FirstOrDefault(t => t.TableId == table.TableId);
        }

        /// <summary>
        /// Logic cho nút "Dọn"
        /// </summary>
        private void ClearTable(object obj)
        {
            if (obj is not Table table) return;

            // XAML đã ẩn nút này nếu Status == 0, nhưng kiểm tra lại cho chắc
            if (table.Status == 0)
            {
                MessageBox.Show("Bàn này đã trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            table.Status = 0; // 0 = Trống
            _tableService.Update(table);

            MessageBox.Show($"Đã dọn và cập nhật bàn '{table.Name}' thành Trống",
                "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadTables(); // Tải lại danh sách
            SelectedTable = Tables.FirstOrDefault(t => t.TableId == table.TableId);
        }

        /// <summary>
        /// Logic cho nút "Thêm Bàn"
        /// </summary>
        private void AddTable(object obj)
        {
            // TODO: Mở một cửa sổ (Dialog) để thêm bàn mới tại đây
            MessageBox.Show("Chức năng 'Thêm Bàn' đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Sau khi dialog thêm bàn đóng, bạn có thể gọi LoadTables() để cập nhật
            // LoadTables();
        }

        private string GetStatusText(byte s) => s switch
        {
            0 => "Trống",
            1 => "Có khách",
            2 => "Đặt trước",
            3 => "Đang dọn",
            4 => "Khóa",
            _ => "Không xác định"
        };
    }
}
