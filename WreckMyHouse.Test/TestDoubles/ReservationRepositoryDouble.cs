using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.Test.TestDoubles
{
    public class ReservationRepositoryDouble : IReservationRepository
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public ReservationRepositoryDouble()
        {
            Reservation reservation = new Reservation();
            reservation.Id = 18;
            reservation.StartDate = DateTime.Parse("4/14/2022");
            reservation.EndDate = DateTime.Parse("4/15/2022");
            reservation.GuestId = 11;
            reservation.Total = 511.25m;
        }
        Host host = new Host
        {
            Id = "293508da-e367-437a-9178-1ebaa7d83015",
            LastName = "McLurg",
            Email = "nmclurg1s@umn.edu",
            Phone = "(909) 6821531",
            Address = "8542 Commercial Point",
            City = "Riverside",
            State = "CA",
            PostalCode = "92505",
            StandardRate = 409m,
            WeekendRate = 511.25m
        };

        public Reservation Add(Reservation reservation, Host host)
        {
            List<Reservation> all = FindHostReservationById(host.Id);
            all.Add(reservation);
            return reservation;
        }

        public bool Delete(int reservationId, Host host)
        {
            List<Reservation> reservations = FindHostReservationById(host.Id);
            foreach (var reservation in reservations.Where(reservation => reservation.Id == reservationId))
            {
                reservations.Remove(reservation);
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

        public bool Update(Reservation oldReservation, Reservation newReservation, Host host)
        {
            List<Reservation> reservations = FindHostReservationById(host.Id);
            for (int i = 0; i < reservations.Count; i++)
            {
                if (oldReservation.Id == reservations[i].Id)
                {
                    reservations[i] = newReservation;
                    return true;
                }
            }
            return false;
        }

        private string GetFilePath(string hostId)
        {
            return Path.Combine(projectDirectory, $"{hostId}.csv");
        }
        private Reservation Deserialize(string[] fields)
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

    }
}