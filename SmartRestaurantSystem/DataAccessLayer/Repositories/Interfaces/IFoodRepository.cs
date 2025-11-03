using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IFoodRepository
    {
        List<Food> GetAll();
        Food GetById(int id);
        void Add(Food food);
        void Update(Food food);
        void Delete(int id);
    }
}
