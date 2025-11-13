using BusinessObjects.Models;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Implementations
{
    public class FoodRepository : IFoodRepository
    {
        public List<Food> GetAll()
        {
            using var context = new SmartRestaurantDbContext();
            return context.Foods
                .Include(f => f.Category)
                .AsNoTracking() 
                .Where(f => f.IsAvailable == true)
                .OrderBy(f => f.FoodId)
                .ToList();
        }

        public Food GetById(int id)
        {
            using var context = new SmartRestaurantDbContext();
            return context.Foods
                .AsNoTracking()
                .FirstOrDefault(f => f.FoodId == id);
        }

        public void Add(Food food)
        {
            using var context = new SmartRestaurantDbContext();
            context.Foods.Add(food);
            context.SaveChanges();
        }

        public void Update(Food food)
        {
            using var context = new SmartRestaurantDbContext();
            context.Foods.Update(food);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = new SmartRestaurantDbContext();
            var f = context.Foods.Find(id);
            if (f != null)
            {
                f.IsAvailable = false;                    // ✅ Xóa mềm
                context.SaveChanges();
            }
        }

        public List<Food> GetAllIncludingDeleted()
        {
            using var context = new SmartRestaurantDbContext();
            return context.Foods
                .Include(f => f.Category)
                .AsNoTracking()
                .OrderBy(f => f.FoodId)
                .ToList();
        }

        public void Restore(int id)
        {
            using var context = new SmartRestaurantDbContext();
            var f = context.Foods.Find(id);
            if (f != null)
            {
                f.IsAvailable = true;  // ✅ Bật lại món ăn
                context.SaveChanges();
            }
        }

    }
}

