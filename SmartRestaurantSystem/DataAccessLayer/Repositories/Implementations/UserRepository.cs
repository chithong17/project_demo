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
    public class UserRepository : IUserRepository
    {
        public List<User> GetAll()
        {
            using var context = new SmartRestaurantDbContext();
            return context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .ToList();
        }

        public User GetById(int id)
        {
            using var context = new SmartRestaurantDbContext();
            return context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefault(u => u.UserId == id);
        }

        public User GetByUsername(string username)
        {
            using var context = new SmartRestaurantDbContext();
            return context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefault(u => u.Username == username);
        }

        public void Add(User user)
        {
            using var context = new SmartRestaurantDbContext();

            if (string.IsNullOrWhiteSpace(user.Password))
                user.Password = "123456";

            if (user.Role != null)
            {
                user.RoleId = user.Role.RoleId;
                user.Role = null;
            }

            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Update(User user)
        {
            using var context = new SmartRestaurantDbContext();

            if (user.Role != null)
            {
                user.RoleId = user.Role.RoleId;
                user.Role = null;
            }

            context.Users.Update(user);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = new SmartRestaurantDbContext();
            var user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
    }
}
