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

namespace RestaurantWPF.ViewModels.Admin
{
    public class AdminViewModel : BaseViewModel
    {
        public string AdminName => UserSession.FullName ?? "Administrator";
        public string Greeting => $"Xin chào, {AdminName} 👋";

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowFoodsCommand { get; }
        public ICommand ShowTablesCommand { get; }
        public ICommand ShowOrdersCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowFeedbackStatsCommand { get; }

        public ICommand LogoutCommand { get; }

        public AdminViewModel()
        {
            CurrentView = new DashboardViewModel();

            ShowDashboardCommand = new RelayCommand(_ => CurrentView = new DashboardViewModel());
            ShowFoodsCommand = new RelayCommand(_ => CurrentView = new FoodViewModel());
            ShowTablesCommand = new RelayCommand(_ => CurrentView = new TableViewModel());
            ShowOrdersCommand = new RelayCommand(_ => CurrentView = new OrderViewModel());
            ShowUsersCommand = new RelayCommand(_ => CurrentView = new UserManagementViewModel());
            ShowFeedbackStatsCommand = new RelayCommand(_ => ShowFeedbackStatistics());

            // Logout
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        private void ShowFeedbackStatistics()
        {
            CurrentView = new FeedbackStatisticsViewModel();
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
