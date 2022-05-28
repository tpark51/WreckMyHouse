using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.DAL
{
    public class GuestRepository : IGuestRepository 
    {
        private const string HEADER = "guest_id,first_name,last_name,email,phone,state";
        private readonly string filePath;

        public GuestRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public List<Guest> FindAll()
        {
            var list = new List<Guest>();
            if(!File.Exists(filePath))
            {
                return list;
            }
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch(Exception ex)
            {
                throw new Exception("could not read guest list", ex);
            }

            for(int i = 1; i < lines.Length; i++) //use 1 to skip the header
            {
                string[] fields = lines[i].Split(",", StringSplitOptions.TrimEntries);
                Guest guest = Deserialize(fields);
                if(guest != null)
                {
                    list.Add(guest);
                }
            }
            return list;
        }

        private Guest Deserialize(string[] fields)
        {
            try
            {
                if (fields.Length != 6)
                {
                    return null;
                }
                Guest guest = new Guest();
                guest.Id = int.Parse(fields[0]);
                guest.FirstName = fields[1];
                guest.LastName = fields[2];
                guest.Email = fields[3];
                guest.Phone = fields[4];
                guest.State = fields[5];
                return guest;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not deserialize guest", ex);
            }

        }
    }
}
