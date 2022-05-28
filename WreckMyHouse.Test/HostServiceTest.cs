using NUnit.Framework;
using WreckMyHouse.BLL;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Test.TestDoubles;

namespace WreckMyHouse.Test
{
    public class HostServiceTest
    {
        HostService hostService = new HostService(new HostRepositoryDouble());

        [Test]
        public void ShouldFindRhodes()
        {
            Host Rhodes = hostService.FindHostByEmail("krhodes1@posterous.com");
            Assert.NotNull(Rhodes);
            Assert.AreEqual("a0d911e7-4fde-4e4a-bdb7-f047f15615e8", Rhodes.Id);
            Assert.AreEqual("Rhodes", Rhodes.LastName);
            Assert.AreEqual("krhodes1@posterous.com", Rhodes.Email);
            Assert.AreEqual("(478) 7475991", Rhodes.Phone);
            Assert.AreEqual("7262 Morning Avenue", Rhodes.Address);
            Assert.AreEqual("Macon", Rhodes.City);
            Assert.AreEqual("GA", Rhodes.State);
            Assert.AreEqual("31296", Rhodes.PostalCode);
            Assert.AreEqual(295, Rhodes.StandardRate);
            Assert.AreEqual(368.75m, Rhodes.WeekendRate);
        }

    }
}
