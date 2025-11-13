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

        private List<User> _allUsers; // ✅ Danh sách gốc

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter(); // Tự động lọc mỗi khi gõ
            }
        }

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

        private void ApplyFilter()
        {
            if (_allUsers == null) return;

            Users.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allUsers
                : _allUsers.Where(u =>
                    (u.Username != null && u.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (u.FullName != null && u.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (u.Email != null && u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (u.Phone != null && u.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (u.Role != null && u.Role.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                ).ToList();

            foreach (var user in filtered)
                Users.Add(user);
        }


        private void LoadUsers()
        {
            _allUsers = _userService.GetAllActiveCustomers();
            ApplyFilter(); 
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

            if (MessageBox.Show("Bạn có chắc muốn xóa người dùng này không?",
                                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _userService.SoftDeleteCustomer(SelectedUser.UserId);
                LoadUsers();
                MessageBox.Show("Người dùng đã được ẩn (xóa mềm) khỏi hệ thống.",
                                "Đã xóa", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
