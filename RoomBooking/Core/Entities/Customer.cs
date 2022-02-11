using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{

    [Index(nameof(EmailAddress), IsUnique = true)]

    public class Customer : EntityObject
    {
        public string? FirstName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string CreditCardNumber { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public string FullName => $"{LastName} {FirstName}";

        public override string ToString()
        {
            return $"{ LastName} {FirstName} (Email: {EmailAddress} CC: {CreditCardNumber})";
        }

    }
}
