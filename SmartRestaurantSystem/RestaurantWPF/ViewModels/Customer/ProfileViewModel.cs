using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using Microsoft.Win32;
using RestaurantWPF.Commands;
using RestaurantWPF.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantWPF.ViewModels.Customer
{
    public class ProfileViewModel : ObservableObject
    {
        private readonly IUserService _userService = new UserService();
        private readonly IReservationService _reservationService = new ReservationService();
        private readonly IFeedbackService _feedbackService = new FeedbackService();

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        private string _avatarUrl;
        public string AvatarUrl
        {
            get => _avatarUrl;
            set { _avatarUrl = value; OnPropertyChanged(); }
        }

        // Statistics
        private int _reservationCount;
        public int ReservationCount
        {
            get => _reservationCount;
            set { _reservationCount = value; OnPropertyChanged(); }
        }

        private int _feedbackCount;
        public int FeedbackCount
        {
            get => _feedbackCount;
            set { _feedbackCount = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand SelectAvatarCommand { get; }

        public ProfileViewModel()
        {
            SaveCommand = new RelayCommand(_ => SaveProfile());
            SelectAvatarCommand = new RelayCommand(_ => SelectAvatar());

            LoadProfile();
        }


        private void SelectAvatar()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Chọn ảnh đại diện",
                Filter = "Ảnh (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                AvatarUrl = dlg.FileName; // gán đường dẫn local
                OnPropertyChanged(nameof(AvatarUrl));
            }
        }
        private void LoadProfile()
        {
            CurrentUser = _userService.GetById(UserSession.UserId);

            if (CurrentUser != null)
            {
                FullName = CurrentUser.FullName;
                Phone = CurrentUser.Phone ?? "";
                AvatarUrl = string.IsNullOrWhiteSpace(CurrentUser.AvatarUrl)
                    ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                    : CurrentUser.AvatarUrl;

                // Load statistics
                ReservationCount = _reservationService.GetAll().Count(r => r.CustomerId == UserSession.UserId);
                FeedbackCount = _feedbackService.GetAll().Count(f => f.CustomerId == UserSession.UserId);
            }
        }

        private void SaveProfile()
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                MessageBox.Show("Tên không được để trống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentUser.FullName = FullName;
            CurrentUser.Phone = Phone;
            CurrentUser.AvatarUrl = AvatarUrl;

            _userService.Update(CurrentUser);
            MessageBox.Show("Cập nhật hồ sơ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
