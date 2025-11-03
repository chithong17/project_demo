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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantWPF.Views.Admin.Dialogs
{
    /// <summary>
    /// Interaction logic for TableDialog.xaml
    /// </summary>
    public partial class TableDialog : Window
    {
        public TableDialogViewModel ViewModel { get; }

        public TableDialog()
        {
            InitializeComponent();
            ViewModel = new TableDialogViewModel();
            DataContext = ViewModel;
        }

        public TableDialog(Table tableToEdit)
        {
            InitializeComponent();
            ViewModel = new TableDialogViewModel(tableToEdit);
            DataContext = ViewModel;
        }
    }
}
