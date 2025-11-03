using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByUsername(string username);
        void Add(User u);
        void Update(User u);
        void Delete(int id);

        List<User> GetCustomers();
        List<User> GetStaffs();
    }
}
