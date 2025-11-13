using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantWPF.ViewModels.Staff
{
    public class OrderDetailViewModel : INotifyPropertyChanged
    {
        // Biến này chứa Model gốc (lấy từ database)
        private readonly OrderDetail _model;

        // Hàm khởi tạo, nhận vào Model
        public OrderDetailViewModel(OrderDetail model)
        {
            _model = model;
        }

        // Cung cấp các thuộc tính mà XAML cần binding

        public int OrderDetailId => _model.OrderDetailId;
        public Food Food => _model.Food;
        public decimal UnitPrice => _model.UnitPrice;

        // Đây là thuộc tính XAML binding tới
        public string? Note
        {
            get => _model.Note;
            set
            {
                if (_model.Note != value)
                {
                    _model.Note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        // ****** ĐÂY LÀ PHẦN SỬA LỖI QUAN TRỌNG NHẤT ******
        // Property Quantity này sẽ thông báo cho UI khi nó thay đổi
        public int Quantity
        {
            get => _model.Quantity;
            set
            {
                // Chỉ set và thông báo nếu giá trị thực sự thay đổi
                if (_model.Quantity != value && value > 0) // Thêm check > 0 nếu muốn
                {
                    _model.Quantity = value;
                    OnPropertyChanged(nameof(Quantity)); // Báo cho UI cập nhật!
                }
            }
        }

        // Lấy Model gốc ra (nếu cần)
        public OrderDetail GetModel() => _model;


        // --- Triển khai INotifyPropertyChanged ---
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
