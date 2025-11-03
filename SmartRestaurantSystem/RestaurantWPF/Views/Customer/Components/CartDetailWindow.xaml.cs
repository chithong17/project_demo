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
using System.Windows.Shapes;

namespace RestaurantWPF.Views.Customer.Components
{
    /// <summary>
    /// Interaction logic for CartDetailWindow.xaml
    /// </summary>
    public partial class CartDetailWindow : Window
    {
        public CartDetailWindow()
        {
            InitializeComponent();
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm && vm.CartItems.Any())
            {
                try
                {
                    vm.PlaceOrder();
                    MessageBox.Show("Cảm ơn bạn! Đơn hàng của bạn đã được ghi nhận 🍽️",
                        "Đặt món thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đặt món: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Giỏ hàng đang trống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
