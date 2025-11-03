using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        List<Reservation> GetAll();
        Reservation GetById(int id);
        List<Reservation> GetUpcoming();
        void Add(Reservation reservation);
        void Update(Reservation reservation);
        void Delete(int id);
    }
}
