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
    public class UserManagementViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        public ObservableCollection<User> Users { get; set; }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public UserManagementViewModel()
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>();

            LoadCommand = new RelayCommand(_ => LoadUsers());
            AddCommand = new RelayCommand(_ => AddUser());
            EditCommand = new RelayCommand(_ => EditUser(), _ => SelectedUser != null);
            DeleteCommand = new RelayCommand(_ => DeleteUser(), _ => SelectedUser != null);

            LoadUsers();
        }

        private void LoadUsers()
        {
            Users.Clear();
            var list = _userService.GetAll();
            foreach (var user in list)
                Users.Add(user);
        }

        private void AddUser()
        {
            var dialog = new UserDialog();
            if (dialog.ShowDialog() == true)
            {
                var newUser = dialog.ViewModel.CurrentUser;
                _userService.Add(newUser);
                LoadUsers();
                MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditUser()
        {
            if (SelectedUser == null) return;

            var dialog = new UserDialog(SelectedUser);
            if (dialog.ShowDialog() == true)
            {
                var updatedUser = dialog.ViewModel.CurrentUser;
                _userService.Update(updatedUser);
                LoadUsers();
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser == null) return;

            if (MessageBox.Show("Delete this user?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _userService.Delete(SelectedUser.UserId);
                LoadUsers();
                MessageBox.Show("User deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
