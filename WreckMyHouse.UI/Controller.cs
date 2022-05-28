using System;
using System.Collections.Generic;
using System.Linq;
using WreckMyHouse.BLL;
using WreckMyHouse.Core.Models;

namespace WreckMyHouse.UI
{
    public class Controller
    {
        private readonly View view;
        private readonly ReservationService reservationService;
        private readonly HostService hostService;
        private readonly GuestService guestService;

        public Controller(View view, ReservationService reservationService, HostService hostService, GuestService guestService)
        {
            this.view = view;
            this.reservationService = reservationService;
            this.hostService = hostService;
            this.guestService = guestService;
        }
        public void Run()
        {
            view.DisplayHeader("Welcome to Don't Wreck My House");
            try
            {
                RunAppLoop();
            }
            catch (Exception ex)
            {
                view.DisplayException(ex);
            }
            view.DisplayHeader("Goodbye");
        }
        private void RunAppLoop()
        {
            MainMenuOptions option;
            do
            {
                option = view.SelectMainMenuOption();
                switch (option)
                {
                    case MainMenuOptions.ViewReservationsForHost:
                        ViewByHost();
                        break;
                    case MainMenuOptions.MakeAReservation:
                        MakeReservation();
                        break;
                    case MainMenuOptions.EditAReservation:
                        EditReservation();
                        break;
                    case MainMenuOptions.CancelAReservation:
                        CancelReservation();
                        break;
                    default:
                        break;
                }
            } while (option != MainMenuOptions.Exit);
        }
        private void ViewByHost()
        {
            view.DisplayHeader(MainMenuOptions.ViewReservationsForHost.ToLabel());
            int choice = view.GetAnswer("Do you want view by\n1. Location\n2. Host\nSelect [1-2]: ");
            if(choice == 1) //  TODO: add validation to only allow 1 or 2
            {
                string state = view.GetState("What state would you like to view? ");
                List<Host> hostsByState = hostService.FindHostsByState(state);
                view.DisplayHosts(hostsByState.OrderBy(h => h.City).ThenBy(n => n.LastName).ToList());

                view.EnterToContinue();

            }
            if (choice == 2)
            {
                Host host = hostService.FindHostByEmail(view.GetEmail("Host Email: "));
                if (host == null)
                {
                    Console.WriteLine("No host was found.\n");
                    view.EnterToContinue();
                }
                else
                {
                    List<Reservation> reservations = reservationService.FindAllHostReservations(host.Id);

                    view.DisplayHeader($"{host.LastName}'s Reservations");
                    Console.WriteLine($"Location: {host.City}, {host.State}\n");
                    view.DisplayReservations(reservations.OrderBy(r => r.StartDate).ToList());

                    view.EnterToContinue();
                }
            }
        }
        private void MakeReservation()
        {
            view.DisplayHeader(MainMenuOptions.MakeAReservation.ToLabel());
            Guest guest = GetGuest("Guest Email: ");
            Host host = GetHost();

            List<Reservation> reservations = reservationService.FindAllHostReservations(host.Id);

            view.DisplayHeader("Reservations");
            view.DisplayReservations(reservations.OrderBy(r => r.StartDate).ToList());

            DateTime startDate = GetDateFromUser("Start: ");
            DateTime endDate = GetDateFromUser("End: ");
            bool dateAval = reservationService.ValidateDates(startDate, endDate, host);
            if (!dateAval)
            {
                return;
            }

            var totalCost = reservationService.GetTotalCost(host, startDate, endDate);

            bool confirm = view.DisplayReservationSummary(startDate, endDate, totalCost);
            if (confirm == false)
            {
                Console.WriteLine("No reservation made.");
                view.EnterToContinue();
            }
            else
            {
                Reservation reservation2Add = reservationService.MakeReservation(host, guest, startDate, endDate);
                Result<Reservation> result = reservationService.Add(reservation2Add, host);
                if (!result.Success)
                {
                    view.DisplayStatus(false, result.Messages);
                }
                else
                {
                    string successMessage = $"Reservation {result.Value.Id} created."; 
                    view.DisplayStatus(true, successMessage);
                    view.EnterToContinue();
                }
            }
        }
        private void EditReservation()
        {
            view.DisplayHeader(MainMenuOptions.EditAReservation.ToLabel());
            Guest guest = GetGuest("Guest Email: ");
            Host host = GetHost();

            Console.WriteLine("");
            List<Reservation> reservations = reservationService.FindAllGuestReservations(guest, host);
            view.DisplayReservations(reservations.OrderBy(r => r.StartDate).ToList());

            int selectedId = view.GetInt("\nReservation ID: ");

            Console.WriteLine("");
            Reservation reservation2Update = reservationService.FindOneGuestReservation(selectedId, host.Id);
            view.DisplayReservations(reservation2Update);

            
            DateTime? updatedStartDate = view.GetNewDate($"\nStart ({reservation2Update.StartDate:MM/dd/yyyy}) : ", reservation2Update.StartDate);
            if (updatedStartDate == null)
            {
                updatedStartDate = reservation2Update.StartDate;
            }
            DateTime? updatedEndDate = view.GetNewDate($"End ({reservation2Update.EndDate:MM/dd/yyyy}): ", reservation2Update.EndDate);
            if (updatedEndDate == null)
            {
                updatedEndDate= reservation2Update.EndDate;
            }

            //TODO: Allow updated to be within the same date range of current booking.
            bool dateAval = reservationService.ValidateDates((DateTime)updatedStartDate, (DateTime)updatedEndDate, host);
            if (!dateAval)
            {
                return;
            }

            var newTotalCost = reservationService.GetTotalCost(host, (DateTime)updatedStartDate, (DateTime)updatedEndDate);
            bool confirm = view.DisplayReservationSummary((DateTime)updatedStartDate, (DateTime)updatedEndDate, newTotalCost);
            if (confirm == false)
            {
                Console.WriteLine("Reservation updated canceled. ");
                view.EnterToContinue();
            }
            else
            {        
                Reservation newReservation = reservationService.UpdateReservation((DateTime)updatedStartDate, (DateTime)updatedEndDate, newTotalCost, reservation2Update);

                bool updatedSuccess = reservationService.Update(reservation2Update, newReservation, host);
                if (updatedSuccess == false)
                {
                    view.DisplayStatus(false, "Reservation was not updated.");
                    view.EnterToContinue();
                }
                else
                {
                    string successMessage = $"Reservation {selectedId} has been updated.";
                    view.DisplayStatus(true, successMessage);
                    view.EnterToContinue();
                }
            }

        }
        private void CancelReservation()
        {
            view.DisplayHeader(MainMenuOptions.CancelAReservation.ToLabel());
            Guest guest = guestService.FindGuestByEmail(view.GetEmail("Guest Email: "));
            if (guest == null)
            {
                Console.WriteLine("Guest not found.");
                return;
            }
            Host host = hostService.FindHostByEmail(view.GetEmail("Host Email: "));
            if (host == null)
            {
                return;
            }

            List<Reservation> reservations = reservationService.FindAllGuestReservations(guest, host);
            view.DisplayReservations(reservations.Where(r => r.StartDate >= DateTime.Now).OrderBy(r => r.StartDate).ToList());

            int selectedId = view.GetInt("Reservation ID: ");

            bool deleteSuccess = reservationService.CancelReservation(selectedId, host);
            if (deleteSuccess == false)
            {
                view.DisplayStatus(false, "Reservation was not deleted.");
            }
            else
            {
                string successMessage = $"Reservation {selectedId} deleted.";
                view.DisplayStatus(true, successMessage);
            }
        }
        private Host GetHost()
        {
            int choice = view.GetAnswer("Do you want book using a \n1. Location\n2. Host\nSelect [1-2]: ");
            if (choice == 1) 
            {
                string state = view.GetState("What state would you like to view? ");
                List<Host> hostsByState = hostService.FindHostsByState(state);
                view.DisplayHosts(hostsByState.OrderBy(h => h.City).ThenBy(n => n.LastName).ToList());

                while (true)
                {
                    Host host = hostService.FindHostByEmail(view.GetEmail("Enter host email to start booking: "));
                    if (host != null)
                    {
                        return host;
                    }
                    Console.WriteLine("Host not found.");
                }
            }
            if (choice == 2)
            {
                while (true)
                {
                    Host host = hostService.FindHostByEmail(view.GetEmail("Enter host email to start booking: "));
                    if (host != null)
                    {
                        return host;
                    }
                    Console.WriteLine("Host not found.");
                }
            }
            return null;
        }
        private DateTime GetDateFromUser(string message)
        {
            //DateTime input = view.GetDate("Start: ");
            while (true)
            {
                DateTime input = view.GetDate(message);
                if (input >= DateTime.Now)
                {
                    return input;
                }
                Console.WriteLine("Date must be in the future.");
            }
        }
        private Guest GetGuest(string message)
        {
            while (true)
            {
                Guest guest = guestService.FindGuestByEmail(view.GetEmail(message));
                if (guest != null)
                {
                    return guest;
                }
                Console.WriteLine("Guest not found.");
            }
        }
}

}
