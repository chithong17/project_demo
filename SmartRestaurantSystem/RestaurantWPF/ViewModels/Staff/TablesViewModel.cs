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

        // Command
        public ICommand RefreshCommand { get; }
        public ICommand ChangeStatusCommand { get; }

        public TablesViewModel()
        {
            RefreshCommand = new RelayCommand(_ => LoadTables());
            ChangeStatusCommand = new RelayCommand(ChangeStatus);
            LoadTables();
        }

        private void LoadTables()
        {
            Tables.Clear();
            foreach (var t in _tableService.GetAll())
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
