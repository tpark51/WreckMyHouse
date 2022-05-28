using System;
using System.Collections.Generic;
using System.IO;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.DAL
{
    public class HostRepository : IHostRepository
    {
        private const string HEADER = "id,last_name,email,phone,address,city,state,postal_code,standard_rate,weekend_rate";
        private readonly string filePath;

        public HostRepository(string filePath)
        {
            this.filePath = filePath;
        }
        public List<Host> FindAll()
        {
            var hosts = new List<Host>();
            if(!File.Exists(filePath))
            {
                return hosts;
            }

            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch (Exception ex) //change to custom expection
            {
                throw new Exception("could not read host", ex);
            }

            for(int i = 1; i < lines.Length; i++) //skips the header
            {
                string[] fields = lines[i].Split(",", StringSplitOptions.TrimEntries);
                Host host = Deserialize(fields);
                if(host != null)
                {
                    hosts.Add(host);
                }
            }
            return hosts;
        }

        private Host Deserialize(string[] fields)
        {
            try
            {
                if (fields.Length != 10)
                {
                    return null;
                }

                Host result = new Host();
                result.Id = fields[0];
                result.LastName = fields[1];
                result.Email = fields[2];
                result.Phone = fields[3];
                result.Address = fields[4];
                result.City = fields[5];
                result.State = fields[6];
                result.PostalCode = fields[7];
                result.StandardRate = decimal.Parse(fields[8]);
                result.WeekendRate = decimal.Parse(fields[9]);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not deserialize host", ex);
            }
        }

    }
}
