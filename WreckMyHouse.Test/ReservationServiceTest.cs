using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WreckMyHouse.BLL;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Test.TestDoubles;

namespace WreckMyHouse.Test
{
    public class ReservationServiceTest
    {
        ReservationService reservationService = new ReservationService(
            new ReservationRepositoryDouble(),
            new HostRepositoryDouble(),
            new GuestRepositoryDouble());

        [Test]
        public void ShouldAdd()
        {
            Reservation reservation = new Reservation();

            Host host = HostRepositoryDouble.HOST;

            Result<Reservation> result = reservationService.Add(reservation, host);
            Assert.IsTrue(result.Success);
            Assert.NotNull(result.Value);
            Assert.AreEqual(reservation.Host, host);

        }
    }
}
