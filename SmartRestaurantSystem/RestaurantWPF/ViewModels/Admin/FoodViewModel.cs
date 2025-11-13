using BusinessLogicLayer.Services.Implementations;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using RestaurantWPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantWPF.Views.Admin.Dialogs;

namespace RestaurantWPF.ViewModels.Admin
{
    public class FoodViewModel : BaseViewModel
    {
        private readonly IFoodService _foodService;
        public ObservableCollection<Food> Foods { get; set; }
        private List<Food> _allFoods;

        private Food _selectedFood;
        public Food SelectedFood
        {
            get => _selectedFood;
            set { _selectedFood = value; OnPropertyChanged(); }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand RestoreCommand { get; } // ✅ Mới

        private bool _showHiddenFoods;
        public bool ShowHiddenFoods
        {
            get => _showHiddenFoods;
            set { _showHiddenFoods = value; OnPropertyChanged(); LoadFoods(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter(); // gọi lọc lại mỗi khi người dùng nhập text
            }
        }

        public FoodViewModel()
        {
            _foodService = new FoodService();
            Foods = new ObservableCollection<Food>();
            _allFoods = new List<Food>();
            LoadCommand = new RelayCommand(_ => LoadFoods());
            AddCommand = new RelayCommand(_ => AddFood());
            EditCommand = new RelayCommand(_ => EditFood());
            DeleteCommand = new RelayCommand(_ => DeleteFood());
            RestoreCommand = new RelayCommand(_ => RestoreFood());


            LoadFoods();
        }

        private void LoadFoods()
        {
            var list = _foodService.GetAllIncludingDeleted();
              

            _allFoods = list;
            ApplyFilter();
        }


        private void ApplyFilter()
        {
            Foods.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allFoods
                : _allFoods.Where(f => f.Name.ToLower().Contains(SearchText.ToLower())).ToList();

            foreach (var item in filtered)
                Foods.Add(item);
        }

        private void AddFood()
        {
            var dialog = new Views.Admin.Dialogs.FoodDialog();
            if (dialog.ShowDialog() == true)
            {
                var newFood = dialog.ViewModel.CurrentFood;  
                _foodService.Add(newFood);
                LoadFoods();
            }
        }

        private void EditFood()
        {
            if (SelectedFood == null)
            {
                MessageBox.Show("Vui lòng chọn một món ăn để chỉnh sửa.",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }


            var dialog = new Views.Admin.Dialogs.FoodDialog(SelectedFood); 
            if (dialog.ShowDialog() == true)
            {
                var updatedFood = dialog.ViewModel.CurrentFood;
                _foodService.Update(updatedFood);
                LoadFoods();
                MessageBox.Show("Updated successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteFood()
        {
            if (SelectedFood == null)
            {
                MessageBox.Show("Vui lòng chọn một món ăn để ẩn.",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Ẩn món ăn này khỏi menu?",
                                "Xác nhận",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _foodService.Delete(SelectedFood.FoodId); // xóa mềm
                LoadFoods();
                MessageBox.Show("Món ăn đã được ẩn thành công.",
                                "Thành công",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }


        private void RestoreFood()
        {
            if (SelectedFood == null)
            {
                MessageBox.Show("Vui lòng chọn một món ăn để khôi phục.",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Restore this food to the menu?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _foodService.Restore(SelectedFood.FoodId);
                LoadFoods();
                MessageBox.Show("The food has been restored.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
