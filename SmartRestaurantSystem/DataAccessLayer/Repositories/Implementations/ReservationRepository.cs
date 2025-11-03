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
    public class ReservationRepository : IReservationRepository
    {
        private readonly SmartRestaurantDbContext _context = new();

        public List<Reservation> GetAll() => _context.Reservations.ToList();

        public Reservation GetById(int id) => _context.Reservations.FirstOrDefault(r => r.ReservationId == id);

        public List<Reservation> GetUpcoming() => _context.Reservations.Where(r => r.Status == 0).ToList();

        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }

        public void Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var r = GetById(id);
            if (r != null)
            {
                _context.Reservations.Remove(r);
                _context.SaveChanges();
            }
        }
    }
}
