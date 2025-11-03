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

namespace RestaurantWPF.ViewModels.Admin
{
    public class FoodDialogViewModel : ObservableObject
    {
        private readonly ICategoryService _categoryService;
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public string DialogTitle { get; }

        private Food _currentFood;
        public Food CurrentFood
        {
            get => _currentFood;
            set
            {
                if (_currentFood != value)
                {
                    _currentFood = value;
                    OnPropertyChanged();
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public FoodDialogViewModel()
        {
            _categoryService = new CategoryService();
            Categories = new ObservableCollection<Category>(_categoryService.GetAll().OrderBy(c => c.Name));
            DialogTitle = "Add New Food";

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);

            CurrentFood = new Food
            {
                IsAvailable = true,
                CategoryId = Categories.FirstOrDefault()?.CategoryId ?? 0
            };
        }

        public FoodDialogViewModel(Food foodToEdit)
        {
            _categoryService = new CategoryService();
            Categories = new ObservableCollection<Category>(_categoryService.GetAll().OrderBy(c => c.Name));
            DialogTitle = "Edit Food";

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);

            CurrentFood = new Food
            {
                FoodId = foodToEdit.FoodId,
                Name = foodToEdit.Name,
                Price = foodToEdit.Price,
                CategoryId = foodToEdit.CategoryId,
                Description = foodToEdit.Description,
                IsAvailable = foodToEdit.IsAvailable,
                CreatedAt = foodToEdit.CreatedAt,
                PopularityScore = foodToEdit.PopularityScore
            };
        }

        private bool CanSave(object parameter)
        {
            return CurrentFood != null &&
                   !string.IsNullOrWhiteSpace(CurrentFood.Name) &&
                   CurrentFood.Price >= 0 &&
                   CurrentFood.CategoryId > 0;
        }

        private void Save(object parameter)
        {
            if (parameter is Window dialog)
            {
                dialog.DialogResult = true;
                dialog.Close();
            }
        }

        private void Cancel(object parameter)
        {
            if (parameter is Window dialog)
            {
                dialog.DialogResult = false;
                dialog.Close();
            }
        }
    }
}
