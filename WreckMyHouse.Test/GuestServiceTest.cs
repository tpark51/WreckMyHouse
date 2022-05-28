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
    public class GuestServiceTest
    {
        GuestService guestService = new GuestService(new GuestRepositoryDouble());

        [Test]
        public void ShouldFindSullivanByEmail()
        {
            Guest Sullivan = guestService.FindGuestByEmail("slomas0@mediafire.com");
            Assert.NotNull(Sullivan);
            Assert.AreEqual(1, Sullivan.Id);
            Assert.AreEqual("Sullivan", Sullivan.FirstName);
            Assert.AreEqual("Lomas", Sullivan.LastName);
            Assert.AreEqual("(702) 7768761", Sullivan.Phone);
            Assert.AreEqual("NV", Sullivan.State);
        }
    }
}
