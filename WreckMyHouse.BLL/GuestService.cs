using System.Linq;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.BLL
{
    public class GuestService
    {
        private readonly IGuestRepository guestRepository;

        public GuestService(IGuestRepository guestRepository)
        {
            this.guestRepository = guestRepository;
        }
        public Guest FindGuestByEmail(string guestEmail)
        {
            return guestRepository.FindAll().FirstOrDefault(g => g.Email == guestEmail);
        }
    }
}
