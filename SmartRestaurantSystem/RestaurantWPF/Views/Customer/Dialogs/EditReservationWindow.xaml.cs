using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RestaurantWPF.ViewModels.Customer;

namespace RestaurantWPF.Views.Customer.Dialogs
{
    /// <summary>
    /// Interaction logic for EditReservationWindow.xaml
    /// </summary>
    public partial class EditReservationWindow : Window
    {
        public EditReservationWindow()
        {
            InitializeComponent();
           

            Loaded += (s, e) =>
            {
                if (DataContext is ICloseable vm)
                {
                    vm.RequestClose += () => Dispatcher.Invoke(Close);
                }
            };
        }
    }

    public interface ICloseable
    {
        event Action RequestClose;
    }
}
