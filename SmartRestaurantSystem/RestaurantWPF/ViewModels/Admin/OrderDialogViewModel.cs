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

namespace RestaurantWPF.ViewModels.Admin
{
    public class OrderDialogViewModel : ObservableObject
    {
        private readonly ITableService _tableService;
        private readonly IUserService _userService;

        public ObservableCollection<Table> Tables { get; set; }
        public ObservableCollection<User> Customers { get; set; }
        public ObservableCollection<User> Staffs { get; set; }

        private Order _currentOrder;
        public Order CurrentOrder
        {
            get => _currentOrder;
            set
            {
                _currentOrder = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string DialogTitle { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public OrderDialogViewModel()
        {
            _tableService = new TableService();
            _userService = new UserService();

            Tables = new ObservableCollection<Table>(_tableService.GetAll());
            Customers = new ObservableCollection<User>(_userService.GetCustomers());
            Staffs = new ObservableCollection<User>(_userService.GetStaffs());

            DialogTitle = "Add New Order";

            CurrentOrder = new Order
            {
                OrderTime = DateTime.Now,
                Status = 0,
                Discount = 0
            };

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        public OrderDialogViewModel(Order orderToEdit)
        {
            _tableService = new TableService();
            _userService = new UserService();

            Tables = new ObservableCollection<Table>(_tableService.GetAll());
            Customers = new ObservableCollection<User>(_userService.GetCustomers());
            Staffs = new ObservableCollection<User>(_userService.GetStaffs());

            DialogTitle = "Edit Order";

            CurrentOrder = new Order
            {
                OrderId = orderToEdit.OrderId,
                TableId = orderToEdit.TableId,
                StaffId = orderToEdit.StaffId,
                CustomerId = orderToEdit.CustomerId,
                OrderTime = orderToEdit.OrderTime,
                Discount = orderToEdit.Discount,
                Note = orderToEdit.Note,
                Status = orderToEdit.Status
            };

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave(object parameter)
        {
            return CurrentOrder != null &&
                   CurrentOrder.TableId > 0 &&
                   CurrentOrder.StaffId > 0;
        }

        private void Save(object parameter)
        {
            if (parameter is Window dialog)
            {
                dialog.DialogResult = true;
                dialog.Close();
            }
        }

        private void Cancel(object parameter)
        {
            if (parameter is Window dialog)
            {
                dialog.DialogResult = false;
                dialog.Close();
            }
        }
    }
}
