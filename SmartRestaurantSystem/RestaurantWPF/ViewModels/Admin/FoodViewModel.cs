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

        public FoodViewModel()
        {
            _foodService = new FoodService();
            Foods = new ObservableCollection<Food>();

            LoadCommand = new RelayCommand(_ => LoadFoods());
            AddCommand = new RelayCommand(_ => AddFood());
            EditCommand = new RelayCommand(_ => EditFood(), _ => SelectedFood != null);
            DeleteCommand = new RelayCommand(_ => DeleteFood(), _ => SelectedFood != null);

            LoadFoods();
        }

        private void LoadFoods()
        {
            Foods.Clear();
            var list = _foodService.GetAll();
            foreach (var item in list)
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
            if (SelectedFood == null) return;

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
            if (SelectedFood == null) return;

            if (MessageBox.Show("Delete this food?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _foodService.Delete(SelectedFood.FoodId);
                LoadFoods();
            }
        }
    }
}
