using BusinessObjects.Models;
using RestaurantWPF.ViewModels.Customer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantWPF.Views.Customer.Components
{
    /// <summary>
    /// Interaction logic for FeedbackView.xaml
    /// </summary>
    public partial class FeedbackView : UserControl
    {
        public FeedbackView()
        {
            InitializeComponent();
        }

        private void OnFoodSelected(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Food food)
            {
                var vm = DataContext as FeedbackViewModel;
                vm.SelectedFood = food;
            }
        }

    }
}
