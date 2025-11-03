using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;


namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ITableService
    {
        List<Table> GetAll();
        Table GetById(int id);
        List<Table> GetAvailable();
        void Add(Table t);
        void Update(Table t);
        void Delete(int id);
    }
}
