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
    public class RoleRepository : IRoleRepository
    {
        private readonly SmartRestaurantDbContext _context = new();

        public List<Role> GetAll() => _context.Roles.ToList();

        public Role GetById(int id) => _context.Roles.FirstOrDefault(r => r.RoleId == id);
    }
}
