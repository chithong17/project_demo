using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IFoodService
    {
        List<Food> GetAll();
        Food GetById(int id);
        void Add(Food f);
        void Update(Food f);
        void Delete(int id);
        List<Food> GetByCategory(int categoryId);
    }
}
