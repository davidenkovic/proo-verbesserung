using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Core.Entities
{
    [Index(nameof(RoomNumber), IsUnique = true)]
    public class Room : EntityObject
    { 
       
        public string RoomNumber { get; set; } = string.Empty;

        public RoomType RoomType { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public override string ToString()
        {
            return $"{ RoomNumber} {RoomType}";
        }


    }
}
