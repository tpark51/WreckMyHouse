using System;

namespace WreckMyHouse.UI
{
    public enum MainMenuOptions
    {
        Exit,
        ViewReservationsForHost,
        MakeAReservation,
        EditAReservation,
        CancelAReservation
    }

    public static class MainMenuOptionExtensions
    {
        public static string ToLabel(this MainMenuOptions option) => option switch
        {
            MainMenuOptions.Exit => "Exit",
            MainMenuOptions.ViewReservationsForHost => "View Reservations For Host",
            MainMenuOptions.MakeAReservation => "Make A Reservation ",
            MainMenuOptions.EditAReservation => "Edit A Reservation",
            MainMenuOptions.CancelAReservation => "Cancel A Reservation",
            _ => throw new NotImplementedException() //do i need this?
        };
    }
}
