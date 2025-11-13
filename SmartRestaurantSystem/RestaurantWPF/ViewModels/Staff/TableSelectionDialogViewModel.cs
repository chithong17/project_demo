using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
namespace RestaurantWPF.ViewModels.Staff
{
    public class TableSelectionDialogViewModel : ObservableObject
    {
        public ObservableCollection<SelectableTable> Tables { get; }
        public SelectableTable? SelectedTable => Tables.FirstOrDefault(t => t.IsSelected);

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public TableSelectionDialogViewModel(IEnumerable<Table> tables)
        {
            // Chuyển từ Table sang SelectableTable để thêm IsSelected
            Tables = new ObservableCollection<SelectableTable>(
                tables.Select(t => new SelectableTable
                {
                    TableId = t.TableId,
                    Name = t.Name,
                    Status = t.Status,
                    IsSelected = false
                })
            );

            ConfirmCommand = new RelayCommand<Window>(Confirm);
            CancelCommand = new RelayCommand<Window>(Cancel);
        }

        private void Confirm(Window? window)
        {
            if (SelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn.", "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            window!.Tag = SelectedTable;
            window.DialogResult = true;
            window.Close();
        }

        private void Cancel(Window? window)
        {
            window!.DialogResult = false;
            window.Close();
        }
    }
}
