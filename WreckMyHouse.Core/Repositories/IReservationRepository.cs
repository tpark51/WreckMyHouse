using System.Collections.Generic;
using WreckMyHouse.Core.Models;

namespace WreckMyHouse.Core.Repositories
{
    public interface IReservationRepository
    {
        List<Reservation> FindHostReservationById(string hostId);
        Reservation Add(Reservation reservation, Host host);
        bool Delete(int reservationId, Host host);
        public bool Update(Reservation oldReservation, Reservation newReservation, Host host);
    }
}
