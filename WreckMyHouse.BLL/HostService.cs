using System.Collections.Generic;
using System.Linq;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.BLL
{
    public class HostService
    {
        private readonly IHostRepository hostRepository;

        public HostService(IHostRepository hostRepository)
        {
            this.hostRepository = hostRepository;
        }
        public Host FindHostByEmail(string hostEmail)
        {
            Host host = hostRepository.FindAll().FirstOrDefault(g => g.Email == hostEmail);
            if (host == null)
            {
                return host = null;
            }
            return host;
        }
        public List<Host> FindHostsByState(string state)
        {
            return hostRepository.FindAll().Where(h => h.State == state).ToList();
        }
    }
}
