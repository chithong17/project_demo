using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using RestaurantWPF.Session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Customer
{
    public class ReservationViewModel : ObservableObject
    {
        private readonly IReservationService _reservationService = new ReservationService();

        public ObservableCollection<Reservation> Reservations { get; } = new();
        public DateTime ReservationDate { get; set; } = DateTime.Today;
        public DateTime ReservationTime { get; set; } = DateTime.Now;

        // 🧾 Form fields
        private DateTime _startTime = DateTime.Now.AddHours(1);
        public DateTime StartTime
        {
            get => _startTime;
            set { _startTime = value; OnPropertyChanged(); }
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

        

        public ICommand ReserveCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand EditReservationCommand { get; }


        public ReservationViewModel()
        {
            ReserveCommand = new RelayCommand(_ => AddReservation());
            DeleteReservationCommand = new RelayCommand(DeleteReservation);
            EditReservationCommand = new RelayCommand(EditReservation);

            LoadReservations();
        }

        private void LoadReservations()
        {
            Reservations.Clear();
            var list = _reservationService.GetAll()
                .Where(r => r.CustomerId == UserSession.UserId)
                .OrderByDescending(r => r.CreatedAt);

            foreach (var r in list) Reservations.Add(r);
        }

        private void AddReservation()
        {
            if (NumberOfPeople <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng người hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var reservation = new Reservation
            {
                CustomerId = UserSession.UserId,
                CustomerName = UserSession.FullName,
                Phone = UserSession.Phone,
                StartTime = ReservationDate.Date + ReservationTime.TimeOfDay,
                NumberOfPeople = NumberOfPeople,
                EndTime = StartTime.AddHours(2),
                Status = 0, // Pending
                Note = Note,
                CreatedAt = DateTime.Now
            };

            _reservationService.Add(reservation);
            MessageBox.Show("Đặt bàn thành công! Vui lòng chờ nhân viên xác nhận.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadReservations();
        }

        private void DeleteReservation(object parameter)
        {
            if (parameter is not Reservation reservation)
                return;

            if (!CanModify(reservation))
            {
                MessageBox.Show("Bạn chỉ có thể hủy đặt bàn trước 30 phút so với giờ hẹn!",
                    "Không thể hủy", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Xác nhận hủy đặt bàn vào {reservation.StartTime:g}?",
                "Xác nhận hủy", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                _reservationService.Delete(reservation.ReservationId);
                Reservations.Remove(reservation);

                MessageBox.Show("Đặt bàn của bạn đã được hủy thành công!",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void EditReservation(object parameter)
        {
            if (parameter is not Reservation reservation)
                return;

            if (!CanModify(reservation))
            {
                MessageBox.Show("Bạn chỉ có thể chỉnh sửa đặt bàn trước 30 phút so với giờ hẹn!",
                    "Không thể chỉnh sửa", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Mở cửa sổ chỉnh sửa thật
            var editWindow = new Views.Customer.Dialogs.EditReservationWindow
            {
                DataContext = new EditReservationViewModel(reservation, updated =>
                {
                    _reservationService.Update(updated);
                    LoadReservations();
                    MessageBox.Show("Đặt bàn đã được cập nhật thành công!",
                        "Cập nhật thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                })
            };

            editWindow.ShowDialog();
        }



        private bool CanModify(Reservation reservation)
        {
            // Chỉ cho sửa/hủy nếu còn hơn 30 phút trước giờ hẹn
            var remaining = reservation.StartTime - DateTime.Now;
            return remaining.TotalMinutes > 30 && (reservation.Status == 0 || reservation.Status == 1);
        }

    }
}
