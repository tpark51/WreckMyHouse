using System;
using System.Collections.Generic;
using System.Linq;
using WreckMyHouse.Core.Models;
using WreckMyHouse.Core.Repositories;

namespace WreckMyHouse.BLL
{
    public class ReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IHostRepository hostRepository;
        private readonly IGuestRepository guestRepository;

        public ReservationService(IReservationRepository reservationRepository, IHostRepository hostRepository, IGuestRepository guestRepository)
        {
            this.reservationRepository = reservationRepository; //because the variable have the same name, the compiler doesnt know what one I want. The this lets me gets the instances of this class. This would read the class instances equals the parameter.
            this.hostRepository = hostRepository;
            this.guestRepository = guestRepository;
        }

        public List<Reservation> FindAllHostReservations(string hostId)
        {
            return reservationRepository.FindHostReservationById(hostId);
        }

        public Reservation MakeReservation(Host host, Guest guest, DateTime start, DateTime end)
        {
            Reservation reservation = new Reservation();
            reservation.Host = host;
            reservation.Guest = guest;
            reservation.StartDate = start;
            reservation.EndDate = end;
            reservation.GuestId = guest.Id;
            reservation.Total = GetTotalCost(host, start, end);
            return reservation;
        }

        public Reservation UpdateReservation(DateTime updatedStartDate, DateTime updatedEndDate, decimal newTotalCost, Reservation reservation2Update)
        {
            Reservation newReservation = new Reservation();
            newReservation.StartDate = updatedStartDate;
            newReservation.EndDate = updatedEndDate;
            newReservation.Total = newTotalCost;
            newReservation.Id = reservation2Update.Id;
            newReservation.GuestId = reservation2Update.GuestId;
            return newReservation;
        }

        public Result<Reservation> Add(Reservation reservation, Host host)
        {
            Result<Reservation> result = Validate(reservation);
            if (!result.Success)
            {
                result.Message = "Reservation was not added.";
                return result;
            }

            result.Value = reservationRepository.Add(reservation, host);

            return result;
        }
 
        public bool Update(Reservation oldReservation, Reservation newReservation, Host host)
        {
            return reservationRepository.Update(oldReservation, newReservation, host);
        }
        
        public bool CancelReservation(int reservationId, Host host)
        {
            return reservationRepository.Delete(reservationId, host);
        }

        public List<Reservation> FindAllGuestReservations(Guest guest, Host host)
        {
            List<Reservation> reservations = reservationRepository.FindHostReservationById(host.Id);
            var guestRes = from reservation in reservations
                           where guest.Id == reservation.GuestId
                           select reservation;
            return guestRes.ToList();
        }

        public Reservation FindOneGuestReservation(int reservationId, string hostId)
        {
            List<Reservation> reservations = reservationRepository.FindHostReservationById(hostId);
            var selectedRes = from reservation in reservations
                              where reservation.Id == reservationId
                              select reservation;
            return selectedRes.FirstOrDefault();
        }

        public decimal GetTotalCost(Host host, DateTime start, DateTime end)
        {
            TimeSpan visitLenght = end - start;
            var lengthOfTrip = visitLenght.Days + 1;
            var standardDays = GetNumberOfWeekdays(start, end);
            var weekendDays = lengthOfTrip - standardDays;

            decimal standardDaysTotal = standardDays * host.StandardRate;
            decimal weekendDaysTotal = weekendDays * host.WeekendRate;

            return standardDaysTotal + weekendDaysTotal;  
        }
        
        public bool ValidateDates(DateTime startDate, DateTime endDate, Host host)
        {
                if (startDate < DateTime.Now)
                {
                    Console.WriteLine("Check-In must be in the future.");
                    return false;
                }
                List<DateTime> validDates = BookedDates(host.Id);
                List<DateTime> checkDates = DatesToCheck(startDate, endDate);
                foreach (DateTime date in checkDates)
                {
                    if (validDates.Contains(date))
                    {
                        Console.WriteLine("Date already booked.");
                        return false;
                    }
                }
            return true;
        }

        //Support
        private int GetNumberOfWeekdays(DateTime start, DateTime stop)
        {
            TimeSpan interval = stop - start;
            int totalWeek = interval.Days / 7;
            int totalWeekdays = 5 * totalWeek;

            int remainingDays = interval.Days % 7;

            for (int i = 0; i <= remainingDays; i++)
            {
                DayOfWeek test = (DayOfWeek)(((int)start.DayOfWeek + i) % 7);
                if (test >= DayOfWeek.Monday && test <= DayOfWeek.Friday)
                    totalWeekdays++;
            }

            return totalWeekdays;
        }
        private List<DateTime> BookedDates(string hostId)
        {
            List<DateTime> bookedDays = new List<DateTime>();
            List<Reservation> reservations = reservationRepository.FindHostReservationById(hostId);
            foreach (var reservation in reservations)
            {
                DateTime startDate = reservation.StartDate;
                DateTime endDate = reservation.EndDate;
                while (startDate < endDate)
                {
                    bookedDays.Add(startDate);
                    startDate = startDate.AddDays(1);
                }
            }
            return bookedDays;
        }
        private List<DateTime> DatesToCheck(DateTime startDate, DateTime endDate)
        {
            List<DateTime> bookedDays = new List<DateTime>();
            while (startDate < endDate)
            {
                bookedDays.Add(startDate);
                startDate = startDate.AddDays(1);
            }
            return bookedDays;
        }
        private Result<Reservation> Validate(Reservation reservation)
        {
            Result<Reservation> result = ValidateNulls(reservation);
            if (!result.Success)
            {
                result.Message = "Failed validation ";
                return result;
            }

            return result;
        }
        private Result<Reservation> ValidateNulls(Reservation reservation)
        {
            var result = new Result<Reservation>();

            if (reservation == null)
            {
                result.AddMessage("Nothing to save.");
                return result;
            }
            if (reservation.Host == null)
            {
                result.AddMessage("Host is required.");
            }
            if (reservation.Guest == null)
            {
                result.AddMessage("Guest is required. ");
            }
            return result;
        }        
    }
}

