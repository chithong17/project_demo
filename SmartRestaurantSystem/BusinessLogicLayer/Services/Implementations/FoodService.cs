using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Implementations
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _repo = new FoodRepository();

        public List<Food> GetAll() => _repo.GetAll();
        public Food GetById(int id) => _repo.GetById(id);
        public void Add(Food f) => _repo.Add(f);
        public void Update(Food f) => _repo.Update(f);
        public void Delete(int id) => _repo.Delete(id);
        public List<Food> GetByCategory(int categoryId) => _repo.GetAll().Where(f => f.CategoryId == categoryId).ToList();
    }
}
