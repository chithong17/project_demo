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
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : Window
    {
        public UserDialogViewModel ViewModel { get; }

        public UserDialog()
        {
            InitializeComponent();
            ViewModel = new UserDialogViewModel();
            DataContext = ViewModel;
        }

        public UserDialog(User userToEdit)
        {
            InitializeComponent();
            ViewModel = new UserDialogViewModel(userToEdit);
            DataContext = ViewModel;
        }
    }
}
