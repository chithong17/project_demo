using BusinessLogicLayer.Services.Interfaces;
using BusinessObjects.Models;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repo = new ReservationRepository();
        private readonly ITableRepository _tableRepo = new TableRepository();

        public List<Reservation> GetAll() => _repo.GetAll();
        public Reservation GetById(int id) => _repo.GetById(id);
        public List<Reservation> GetUpcoming() => _repo.GetUpcoming();
        public void Add(Reservation r) => _repo.Add(r);
        public void Update(Reservation r) => _repo.Update(r);
        public void Delete(int id) => _repo.Delete(id);

        public void AssignTable(int reservationId, int tableId)
        {
            var reservation = _repo.GetById(reservationId);
            var table = _tableRepo.GetById(tableId);

            if (reservation == null)
                throw new Exception("Không tìm thấy đơn đặt bàn.");
            if (table == null)
                throw new Exception("Không tìm thấy bàn.");
            if (table.Status != 0)
                throw new Exception("Bàn này không trống.");

            reservation.TableId = tableId;
            reservation.Status = 2; // Confirmed
            table.Status = 2;       // Reserved

            _repo.Update(reservation);
            _tableRepo.Update(table);
        }
    }
}
