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
    public class TableRepository : ITableRepository
    {
        public List<Table> GetAll()
        {
            using var context = new SmartRestaurantDbContext();
            return context.Tables
                .Where(t => !t.IsDeleted)
                .ToList();
        }

        public Table GetById(int id)
        {
            using var context = new SmartRestaurantDbContext();
            return context.Tables
                .AsNoTracking()
                .FirstOrDefault(t => t.TableId == id);
        }

        public List<Table> GetAvailableTables()
        {
            using var context = new SmartRestaurantDbContext();
            return context.Tables
                .AsNoTracking()
                .Where(t => t.Status == 0)
                .ToList();
        }

        public void Add(Table table)
        {
            using var context = new SmartRestaurantDbContext();
            context.Tables.Add(table);
            context.SaveChanges();
        }

        public void Update(Table table)
        {
            using var context = new SmartRestaurantDbContext();
            context.Tables.Update(table);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = new SmartRestaurantDbContext();
            var t = context.Tables.Find(id);
            if (t != null)
            {
                context.Tables.Remove(t);
                context.SaveChanges();
            }
        }
    }
}
