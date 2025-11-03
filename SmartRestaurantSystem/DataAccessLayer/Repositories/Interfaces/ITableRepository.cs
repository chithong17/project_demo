using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;


namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ITableRepository
    {
        List<Table> GetAll();
        Table GetById(int id);
        List<Table> GetAvailableTables();
        void Add(Table table);
        void Update(Table table);
        void Delete(int id);
    }
}
