using RestaurantWPF.Commands;
using RestaurantWPF.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using RestaurantWPF.Views;

namespace RestaurantWPF.ViewModels.Customer
{
    public class CustomerViewModel : BaseViewModel
    {
        public string CustomerName => UserSession.FullName ?? "Customer";
        public string Greeting => $"Xin chào, {CustomerName} 👋";

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

        public ICommand MenuCommand { get; }
        public ICommand ReservationCommand { get; }
        public ICommand FeedbackCommand { get; }
        public ICommand ProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public CustomerViewModel()
        {
            CurrentView = new MenuViewModel();

            MenuCommand = new RelayCommand(_ => CurrentView = new MenuViewModel());
            ReservationCommand = new RelayCommand(_ => CurrentView = new ReservationViewModel());
            FeedbackCommand = new RelayCommand(_ => CurrentView = new FeedbackViewModel());
            ProfileCommand = new RelayCommand(_ => CurrentView = new ProfileViewModel());
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        private void ExecuteLogout(object obj)
        {
            UserSession.Clear();

            Application.Current.Dispatcher.Invoke(() =>
            {
                var login = new LoginWindow();
                login.Show();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w != login)
                        w.Close();
                }
            });
        }
    }

}
