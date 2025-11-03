using RestaurantWPF.Commands;
using RestaurantWPF.Session;
using RestaurantWPF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        public ICommand LogoutCommand { get; }

        public BaseViewModel()
        {
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        private void ExecuteLogout(object obj)
        {
            UserSession.Clear();

            var login = new LoginWindow();
            login.Show();

            foreach (Window w in Application.Current.Windows)
            {
                if (w != login)
                    w.Close();
            }
        }
    }
}
