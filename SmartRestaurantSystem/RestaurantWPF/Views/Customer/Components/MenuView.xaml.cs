using RestaurantWPF.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace RestaurantWPF.Views.Customer.Components
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
        }

        private void CartIcon_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var vm = DataContext as RestaurantWPF.ViewModels.Customer.MenuViewModel;
            if (vm != null)
                vm.OpenCartDetailCommand.Execute(null);
        }


        private void Popup_Opened(object sender, EventArgs e)
        {
            if (sender is Popup popup && popup.Child is Border border)
            {
                var sb = new Storyboard();

                var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250));
                Storyboard.SetTarget(fade, border);
                Storyboard.SetTargetProperty(fade, new PropertyPath("Opacity"));

                var slide = new DoubleAnimation(10, 0, TimeSpan.FromMilliseconds(250));
                Storyboard.SetTarget(slide, border);
                Storyboard.SetTargetProperty(slide, new PropertyPath("(RenderTransform).(TranslateTransform.Y)"));

                sb.Children.Add(fade);
                sb.Children.Add(slide);
                sb.Begin();
            }
        }
    }
}
