using BusinessObjects.Models;
using RestaurantWPF.Commands;
using RestaurantWPF.Views.Customer.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Customer
{
    public class EditReservationViewModel : ObservableObject
    {
        private readonly Reservation _reservation;
        private readonly Action<Reservation> _onSave;

        public EditReservationViewModel(Reservation reservation, Action<Reservation> onSave)
        {
            _reservation = reservation;
            _onSave = onSave;

            // Gán các giá trị ban đầu
            StartTime = reservation.StartTime;
            NumberOfPeople = reservation.NumberOfPeople;
            Note = reservation.Note;
            TimeString = reservation.StartTime.ToString("HH:mm");
            Phone = reservation.Phone;
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => CloseWindow());
        }
        private string _phone;
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        // Thuộc tính binding
        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set { _startTime = value; OnPropertyChanged(); }
        }

        private string _timeString;
        public string TimeString
        {
            get => _timeString;
            set { _timeString = value; OnPropertyChanged(); }
        }

        private int _numberOfPeople;
        public int NumberOfPeople
        {
            get => _numberOfPeople;
            set { _numberOfPeople = value; OnPropertyChanged(); }
        }

        private string _note;
        public string Note
        {
            get => _note;
            set { _note = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save()
        {
            if (TimeSpan.TryParse(TimeString, out var time))
            {
                _reservation.StartTime = StartTime.Date + time;
                _reservation.EndTime = _reservation.StartTime.AddHours(2);
            }

            _reservation.NumberOfPeople = NumberOfPeople;
            _reservation.Note = Note;

            _onSave?.Invoke(_reservation);

            CloseWindow();
        }

        private void CloseWindow()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.DataContext == this)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}
