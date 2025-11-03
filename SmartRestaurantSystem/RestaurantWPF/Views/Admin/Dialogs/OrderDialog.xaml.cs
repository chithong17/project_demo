using BusinessObjects.Models;
using RestaurantWPF.ViewModels.Admin;
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

namespace RestaurantWPF.Views.Admin.Dialogs
{
    /// <summary>
    /// Interaction logic for OrderDialog.xaml
    /// </summary>
    public partial class OrderDialog : Window
    {
        public OrderDialogViewModel ViewModel { get; }

        public OrderDialog()
        {
            InitializeComponent();
            ViewModel = new OrderDialogViewModel();
            DataContext = ViewModel;
        }

        public OrderDialog(Order orderToEdit)
        {
            InitializeComponent();
            ViewModel = new OrderDialogViewModel(orderToEdit);
            DataContext = ViewModel;
        }
    }
}
