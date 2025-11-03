using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Admin
{
    public class TableDialogViewModel : ObservableObject
    {
        private readonly ITableService _tableService;

        private Table _currentTable;
        public Table CurrentTable
        {
            get => _currentTable;
            set
            {
                _currentTable = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string DialogTitle { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public TableDialogViewModel()
        {
            _tableService = new TableService();
            DialogTitle = "Add New Table";
            CurrentTable = new Table
            {
                Name = "",
                Capacity = 4,
                Location = "Main Hall",
                Status = 0
            };
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        public TableDialogViewModel(Table tableToEdit)
        {
            _tableService = new TableService();
            DialogTitle = "Edit Table";
            CurrentTable = new Table
            {
                TableId = tableToEdit.TableId,
                Name = tableToEdit.Name,
                Capacity = tableToEdit.Capacity,
                Location = tableToEdit.Location,
                Status = tableToEdit.Status
            };
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(CurrentTable.Name) && CurrentTable.Capacity > 0;
        }

        private void Save(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void Cancel(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}
