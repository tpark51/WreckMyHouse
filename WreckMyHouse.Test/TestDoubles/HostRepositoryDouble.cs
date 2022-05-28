using System;
using System.Collections.Generic;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.Test.TestDoubles
{
    public class HostRepositoryDouble : IHostRepository
    {
        public static readonly Host HOST = new Host("a0d911e7-4fde-4e4a-bdb7-f047f15615e8", "Rhodes", "krhodes1@posterous.com", "(478) 7475991", "7262 Morning Avenue", "Macon", "GA", "31296", 295, 368.75m);
        private List<Host> hosts = new List<Host>();

        public HostRepositoryDouble()
        {
            hosts.Add(HOST);
        }
        public List<Host> FindAll()
        {
            return new List<Host>(hosts);
        }
    }
}