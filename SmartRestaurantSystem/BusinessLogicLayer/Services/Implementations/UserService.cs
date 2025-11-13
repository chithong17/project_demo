using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo = new UserRepository();

        public List<User> GetAll() => _repo.GetAll();
        public User GetById(int id) => _repo.GetById(id);
        public User GetByUsername(string username) => _repo.GetByUsername(username);
        public void Add(User u) => _repo.Add(u);
        public void Update(User u) => _repo.Update(u);
        public void Delete(int id) => _repo.Delete(id);

        public List<User> GetCustomers() =>
            _repo.GetAll().Where(u => u.Role.Name == "Customer").ToList();

        public List<User> GetStaffs() =>
            _repo.GetAll().Where(u => u.Role.Name == "Staff").ToList();

        public void SoftDeleteCustomer(int customerId)
        {
            using var context = new SmartRestaurantDbContext();
            var customer = context.Users.FirstOrDefault(c => c.UserId == customerId);
            if (customer != null)
            {
                customer.IsDeleted = true;
                context.SaveChanges();
            }
        }

        public List<User> GetAllActiveCustomers()
        {
            using var context = new SmartRestaurantDbContext();
            return context.Users
                .Where(c => !c.IsDeleted)
                .ToList();
        }


    }
}
