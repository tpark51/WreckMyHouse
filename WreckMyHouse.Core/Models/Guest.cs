using System;

namespace WreckMyHouse.Core.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }

        public Guest() { }

        public Guest(int Id, string firstName, string lastName, string email, string phone, string state)
        {
            this.Id = Id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = Phone;
            State = State;
        }
    }
}
