using RestaurantWPF.Commands;
using RestaurantWPF.Session;
using RestaurantWPF.ViewModels.Admin;
using RestaurantWPF.ViewModels.Customer;
using RestaurantWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantWPF.Views.Staff.Components;

namespace RestaurantWPF.ViewModels.Staff
{
    public class StaffViewModel : BaseViewModel
    {
        public string StaffName => UserSession.FullName ?? "Nhân viên";
        public string Greeting => $"Xin chào, {StaffName} 👨‍🍳";

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand PosCommand { get; }
        public ICommand OrdersCommand { get; }
        public ICommand TablesCommand { get; }
        public ICommand PaymentsCommand { get; }
        public ICommand ReservationsCommand { get; }
        public ICommand LogoutCommand { get; }

        // Constructor
        public StaffViewModel()
        {
            // Giao diện mặc định: Orders
            CurrentView = new PosViewModel();
            //// Điều hướng
            PosCommand = new RelayCommand(_ => CurrentView = new PosViewModel());
            OrdersCommand = new RelayCommand(_ => CurrentView = new OrdersViewModel());
            TablesCommand = new RelayCommand(_ => CurrentView = new TablesViewModel());
            PaymentsCommand = new RelayCommand(_ => CurrentView = new PaymentViewModel());
            ReservationsCommand = new RelayCommand(_ => CurrentView = new ReservationViewModel());

            // Logout
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        private void ExecuteLogout(object obj)
        {
            UserSession.Clear();

            Application.Current.Dispatcher.Invoke(() =>
            {
                var login = new LoginWindow();
                login.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != login)
                        window.Close();
                }
            });
        }
    }
}
