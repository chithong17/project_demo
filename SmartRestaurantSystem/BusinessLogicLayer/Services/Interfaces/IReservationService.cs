using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;


namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IReservationService
    {
        List<Reservation> GetAll();
        Reservation GetById(int id);
        List<Reservation> GetUpcoming();
        void Add(Reservation r);
        void Update(Reservation r);
        void Delete(int id);

        void AssignTable(int reservationId, int tableId);
    }
}
