using System.Collections.Generic;
using WreckMyHouse.Core.Models;

namespace WreckMyHouse.Core.Repositories
{
    public interface IHostRepository
    {
        List<Host> FindAll();
    }
}
