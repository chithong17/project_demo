using RestaurantWPF.ViewModels.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BusinessObjects.Models;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantWPF.Views.Staff.Dialogs
{
    /// <summary>
    /// Interaction logic for TableSelectionDialog.xaml
    /// </summary>
    public partial class TableSelectionDialog : Window
    {
        public SelectableTable? SelectedTable { get; private set; }

        public TableSelectionDialog(IEnumerable<Table> tables)
        {
            InitializeComponent();
            DataContext = new TableSelectionDialogViewModel(tables);
        }

        public new bool? ShowDialog()
        {
            bool? result = base.ShowDialog();
            if (result == true && DataContext is TableSelectionDialogViewModel vm)
            {
                SelectedTable = vm.SelectedTable;
            }
            return result;
        }
    }
}
