using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.DAL
{
    public class ReservationRepository : IReservationRepository
    {
        private const string HEADER = "id,start_date,end_date,guest_id,total";
        private readonly string directory;

        public ReservationRepository(string directory)
        {
            this.directory = directory;
        }
        public Reservation Add(Reservation reservation, Host host)
        {
            try
            {
                List<Reservation> all = FindHostReservationById(host.Id);
                reservation.Id = all.Any() ? all.Max(x => x.Id) : 0 + 1; //IM SO PROUD OF THIS!
                all.Add(reservation);
                Write(all, host);
                return reservation;
            }
            catch (Exception ex)
            {
                throw new Exception ("Could not add reservation", ex);
            }
        }
        public bool Update(Reservation oldReservation, Reservation newReservation, Host host)
        {
            List<Reservation> reservations = FindHostReservationById(host.Id);
            for (int i = 0; i < reservations.Count; i++)
            {
                if(oldReservation.Id == reservations[i].Id)
                {
                    reservations[i] = newReservation;
                    Write(reservations, host);
                    return true;
                }
            }
            return false;
        }
        public bool Delete(int reservationId, Host host)
        {   
            List<Reservation> reservations = FindHostReservationById(host.Id);
            foreach (var reservation in reservations.Where(reservation => reservation.Id == reservationId))
            {
                reservations.Remove(reservation);
                Write(reservations, host);
                return true;
            }
            return false;
        }
        public List<Reservation> FindHostReservationById(string hostId)
        {
            var reservations = new List<Reservation>();
            var path = GetFilePath(hostId);

            if (!File.Exists(path))
            {
                return reservations;
            }

            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch (Exception ex)
            {
                throw new Exception("could not read reservations", ex);
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(",", StringSplitOptions.TrimEntries);
                Reservation reservation = Deserialize(fields);
                if (reservation != null)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }


        private string GetFilePath(string hostId)
        {
            try
            {
                return Path.Combine(directory, $"{hostId}.csv");
            }
            catch(Exception ex)
            {
                throw new Exception("Could not find file path. ", ex);
            }

        }
        private void Write(List<Reservation> reservations, Host host)
        {
            try
            {
                using StreamWriter sw = new StreamWriter(GetFilePath(host.Id));
                sw.WriteLine(HEADER);

                if (reservations == null)
                {
                    return;
                }
                foreach (var reservation in reservations)
                {
                    sw.WriteLine(Serialize(reservation));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not write reservation", ex);
            }
        }
        private string Serialize(Reservation reservation)
        {
            try
            {
                return string.Format("{0},{1:MM/dd/yyy},{2:MM/dd/yyy},{3},{4:0.00}",
                    reservation.Id,
                    reservation.StartDate,
                    reservation.EndDate,
                    reservation.GuestId,
                    reservation.Total);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not serialize reservation", ex);
            }
        }
        private Reservation Deserialize(string[] fields)
        {
            try
            {
                if (fields.Length != 5)
                {
                    return null;
                }
                Reservation result = new Reservation();
                result.Id = int.Parse(fields[0]);
                result.StartDate = DateTime.Parse(fields[1]);
                result.EndDate = DateTime.Parse(fields[2]);
                result.GuestId = int.Parse(fields[3]);
                result.Total = decimal.Parse(fields[4].Replace("$", String.Empty));

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not deserialize reservation", ex);
            }
        }
    }
}
