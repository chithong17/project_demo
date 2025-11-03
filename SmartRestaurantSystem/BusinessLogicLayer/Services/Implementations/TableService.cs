using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;


namespace BusinessLogicLayer.Services.Implementations
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _repo = new TableRepository();

        public List<Table> GetAll() => _repo.GetAll();
        public Table GetById(int id) => _repo.GetById(id);
        public List<Table> GetAvailable() => _repo.GetAvailableTables();
        public void Add(Table t) => _repo.Add(t);
        public void Update(Table t) => _repo.Update(t);
        public void Delete(int id) => _repo.Delete(id);
    }
}
