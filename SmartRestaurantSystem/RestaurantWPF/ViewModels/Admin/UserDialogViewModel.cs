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
    public class UserDialogViewModel : ObservableObject
    {
        private readonly IRoleService _roleService;
        public ObservableCollection<Role> Roles { get; set; }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        public string DialogTitle { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public UserDialogViewModel()
        {
            _roleService = new RoleService();
            Roles = new ObservableCollection<Role>(_roleService.GetAll());

            DialogTitle = "Add New User";
            CurrentUser = new User { CreatedAt = DateTime.Now };

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public UserDialogViewModel(User userToEdit)
        {
            _roleService = new RoleService();
            Roles = new ObservableCollection<Role>(_roleService.GetAll());

            DialogTitle = "Edit User";
            CurrentUser = userToEdit;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
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
