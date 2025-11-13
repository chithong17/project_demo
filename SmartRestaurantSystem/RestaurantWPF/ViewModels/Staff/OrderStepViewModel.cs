using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantWPF.ViewModels.Staff
{
    public class OrderStepViewModel : INotifyPropertyChanged
    {
        public string StepName { get; set; }
        public int StepValue { get; set; }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public OrderStepViewModel(string name, int value)
        {
            StepName = name;
            StepValue = value;
        }
    }
}
