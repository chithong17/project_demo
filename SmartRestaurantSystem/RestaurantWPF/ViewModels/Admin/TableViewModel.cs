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
    public class TableViewModel : BaseViewModel
    {
        private readonly ITableService _tableService;
        public ObservableCollection<Table> Tables { get; set; }

        private Table _selectedTable;
        public Table SelectedTable
        {
            get => _selectedTable;
            set { _selectedTable = value; OnPropertyChanged(); }
        }
        private List<Table> _allTables;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter(); // Gọi lọc lại mỗi khi nhập
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public TableViewModel()
        {
            _tableService = new TableService();
            Tables = new ObservableCollection<Table>();

            LoadCommand = new RelayCommand(_ => LoadTables());
            AddCommand = new RelayCommand(_ => AddTable());
            EditCommand = new RelayCommand(_ => EditTable(), _ => SelectedTable != null);
            DeleteCommand = new RelayCommand(_ => DeleteTable(), _ => SelectedTable != null);

            LoadTables();
        }

        private void LoadTables()
        {
            _allTables = _tableService.GetAll();
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            Tables.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allTables
                : _allTables.Where(t =>
                    (t.Name != null && t.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (t.Location != null && t.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    t.Capacity.ToString().Contains(SearchText)
                ).ToList();

            foreach (var table in filtered)
                Tables.Add(table);
        }


        private void AddTable()
        {
            var dialog = new TableDialog();
            if (dialog.ShowDialog() == true)
            {
                var newTable = dialog.ViewModel.CurrentTable;
                _tableService.Add(newTable);
                LoadTables();
                MessageBox.Show("Table added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditTable()
        {
            if (SelectedTable == null) return;

            var dialog = new TableDialog(SelectedTable);
            if (dialog.ShowDialog() == true)
            {
                var updatedTable = dialog.ViewModel.CurrentTable;
                _tableService.Update(updatedTable);
                LoadTables();
                MessageBox.Show("Table updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteTable()
        {
            if (SelectedTable == null) return;

            if (MessageBox.Show("Delete this table?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _tableService.SoftDelete(SelectedTable.TableId);
                LoadTables();
                MessageBox.Show("Table deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
