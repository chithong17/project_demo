using BusinessObjects.Models;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SmartRestaurantDbContext _context = new();

        public List<Category> GetAll() => _context.Categories.ToList();

        public Category GetById(int id) => _context.Categories.FirstOrDefault(c => c.CategoryId == id);

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var c = GetById(id);
            if (c != null)
            {
                _context.Categories.Remove(c);
                _context.SaveChanges();
            }
        }
    }
}
