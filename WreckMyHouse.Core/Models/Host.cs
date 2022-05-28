namespace WreckMyHouse.Core.Models
{
    public class Host
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public decimal StandardRate { get; set; }
        public decimal WeekendRate { get; set; }

        public Host() { }

        public Host(string id, string email, string lastName, string phone, string address, string city, string state, string postalCode, decimal standardRate, decimal weekendRate)
        {
            Id = id;
            Email = email;
            LastName = lastName;
            Phone = phone;
            Address = address;
            City = city;
            State = state;
            PostalCode = postalCode;            
            StandardRate = standardRate;
            WeekendRate = weekendRate;
        }

    }
}
