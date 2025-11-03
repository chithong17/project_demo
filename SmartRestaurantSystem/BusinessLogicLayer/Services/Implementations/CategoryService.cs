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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo = new CategoryRepository();

        public List<Category> GetAll() => _repo.GetAll();
        public Category GetById(int id) => _repo.GetById(id);
        public void Add(Category category) => _repo.Add(category);
        public void Update(Category category) => _repo.Update(category);
        public void Delete(int id) => _repo.Delete(id);
    }
}
