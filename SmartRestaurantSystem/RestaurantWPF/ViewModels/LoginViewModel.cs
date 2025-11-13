using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using RestaurantWPF.Commands;
using RestaurantWPF.Session;
using RestaurantWPF.Views.Admin;
using RestaurantWPF.Views.Customer;
using RestaurantWPF.Views.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace RestaurantWPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        private readonly IUserService _userService;

        public LoginViewModel()
        {
            _userService = new UserService();
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object obj)
        {
            ErrorMessage = string.Empty;

            try
            {
                var user = _userService.GetByUsername(Username);

                if (user == null || user.Password != Password)
                {
                    ErrorMessage = "Invalid username or password.";
                    return;
                }

                

                // ✅ Lưu session
                UserSession.UserId = user.UserId;
                UserSession.FullName = user.FullName;
                UserSession.Username = user.Username;
                UserSession.RoleName = user.Role?.Name;
                UserSession.Phone = user.Phone;

                // ✅ Mở đúng cửa sổ
                Window nextWindow = user.Role?.Name.ToLower() switch
                {
                    "admin" => new AdminWindow(),
                    "staff" => new StaffWindow(),
                    "customer" => new CustomerWindow(),
                    _ => null
                };

                if (nextWindow == null)
                {
                    ErrorMessage = "Unrecognized role.";
                    return;
                }

                nextWindow.Show();

                // Đóng LoginWindow
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is Views.LoginWindow)
                    {
                        w.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
            }
        }
    }
}

