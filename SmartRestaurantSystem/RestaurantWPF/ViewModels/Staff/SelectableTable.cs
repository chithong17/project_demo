using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantWPF.ViewModels.Staff
{
    public class SelectableTable : ObservableObject
    {
        public int TableId { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }
    }
}
