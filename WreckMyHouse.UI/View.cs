using System;
using System.Collections.Generic;
using WreckMyHouse.Core.Models;

namespace WreckMyHouse.UI
{
    public class View
    {
        private readonly ConsoleIO io; 

        public View(ConsoleIO io) 
        {
            this.io = io;  
        }

        public MainMenuOptions SelectMainMenuOption()
        {
            DisplayHeader("Main Menu");
            int min = int.MaxValue;
            int max = int.MinValue;
            MainMenuOptions[] options = Enum.GetValues<MainMenuOptions>();
            for (int i = 0; i < options.Length; i++)
            {
                MainMenuOptions option = options[i];
                io.PrintLine($"{i}. {option.ToLabel()}");
                min = Math.Min(min, i);
                max = Math.Max(max, i);
            }

            string message = $"Select [{min}-{max - 1}]: ";
            return options[io.ReadInt(message, min, max)];
        }
        public string GetEmail(string message)
        {
            return io.ReadRequiredString(message).ToLower();
        }
        public string GetState (string message)
        {
            return io.ReadRequiredString(message).ToUpper();
        }
        public DateTime GetDate(string message)
        {
            return io.ReadDate(message);
        }
        public DateTime GetNewDate(string message, DateTime oldDate)
        {
            return io.ReadNewDate(message, oldDate);
        }
        public int GetInt(string message)
        {
            return io.ReadInt(message);
        }
        public Reservation MakeReservation(Host host, Guest guest)
        {
            Reservation reservation = new Reservation();
            reservation.Host = host;
            reservation.Guest = guest;
            reservation.StartDate = io.ReadDate("Check-In Date (MM/dd/yyyy): ");
            reservation.EndDate = io.ReadDate("Check-Out Date (MM/dd/yyyy): ");
            reservation.GuestId = guest.Id;
            return reservation;
        }
        public int GetAnswer(string message)
        {
            return io.ReadInt(message);
        }


        //Displays
        internal void DisplayException(Exception ex)
        {
            DisplayHeader("A critical error occurred:");
            io.PrintLine(ex.Message);
        }

        public void DisplayHeader(string message)
        {
            io.PrintLine("");
            io.PrintLine(message);
            io.PrintLine(new string('=', message.Length));
        }

        internal void EnterToContinue()
        {
            io.ReadString("Press [Enter] to continue.");
        }

        public void DisplayStatus(bool success, string message)
        {
            DisplayStatus(success, new List<string>() { message });
        }

        public void DisplayStatus(bool success)
        {
            DisplayStatus(success);
        }

        public void DisplayStatus(bool success, List<string> messages)
        {
            DisplayHeader(success ? "Success" : "Error");
            foreach (string message in messages)
            {
                io.PrintLine(message);
            }
        }

        public void DisplayReservations(List<Reservation> reservations)
        {
            if (reservations == null || reservations.Count == 0)
            {
                io.PrintLine("No reservation found.");
                return;
            }

            io.PrintLine("CheckIn Date - CheckOut Date - Reservation ID");
            foreach (Reservation reservation in reservations)
            {
                io.PrintLine(
                    string.Format(" {1:MM/dd/yyyy}  -  {2:MM/dd/yyyy}   -    {0}      ",
                        reservation.Id,
                        reservation.StartDate.Date,
                        reservation.EndDate.Date) 
                );
            }
            io.PrintLine("");
        }
        public void DisplayReservations(Reservation reservation)
        {
            io.PrintLine("CheckIn Date - CheckOut Date - Reservation ID");
            io.PrintLine(
                string.Format(" {1:MM/dd/yyyy}  -  {2:MM/dd/yyyy}   -    {0}      ",
                    reservation.Id,
                    reservation.StartDate.Date,
                    reservation.EndDate.Date)
            );
            io.PrintLine("");
        }
        public void DisplayHosts(List<Host> hosts)
        {
            if (hosts == null || hosts.Count == 0)
            {
                io.PrintLine("No host in that state found.");
                return;
            }

            io.PrintLine(" City, State    -  Host    -  Email");
            foreach (Host host in hosts)
            {
                io.PrintLine(
                    string.Format(" {1}, {2}    -  {0}    -  {3}",
                        host.LastName,
                        host.City,
                        host.State,
                        host.Email)
                );
            }
            io.PrintLine("");
        }
        public bool DisplayReservationSummary(DateTime start, DateTime end, decimal totalCost)
        {
            DisplayHeader("Summary");
            io.PrintLine("");
            io.PrintLine($"Start: {start:MM/dd/yyyy}");
            io.PrintLine($"  End: {end:MM/dd/yyyy}");
            io.PrintLine($"Total: ${totalCost}\n");
            return io.ReadBool("Do you still want to book your reservation? [y/n]: ");
        }

    }
}
    
