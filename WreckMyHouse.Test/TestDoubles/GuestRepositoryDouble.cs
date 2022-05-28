using System.Collections.Generic;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.Test.TestDoubles
{
    public class GuestRepositoryDouble : IGuestRepository
    {
        public static readonly Guest GUEST = new Guest(1, "Sullivan", "Lomas", "slomas0@mediafire.com", "(702) 7768761", "NV");
        private List<Guest> guests = new List<Guest>();

        public GuestRepositoryDouble()
        {
            guests.Add(GUEST);
        }
        public List<Guest> FindAll()
        {
            return new List<Guest>(guests);
        }

    }
}
