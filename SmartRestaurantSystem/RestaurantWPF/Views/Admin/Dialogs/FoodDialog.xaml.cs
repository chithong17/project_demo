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
    /// Interaction logic for FoodDialog.xaml
    /// </summary>
    public partial class FoodDialog : Window
    {
        public FoodDialogViewModel ViewModel { get; }

        public FoodDialog()
        {
            InitializeComponent();
            ViewModel = new FoodDialogViewModel(); // Add mode
            DataContext = ViewModel;
        }

        public FoodDialog(Food foodToEdit)
        {
            InitializeComponent();
            ViewModel = new FoodDialogViewModel(foodToEdit); // Edit mode
            DataContext = ViewModel;
        }
    }
}
