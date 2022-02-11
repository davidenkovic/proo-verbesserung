using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class RoomDTO
    {
        public string RoomNumber { get; set; } = string.Empty;

        public RoomType RoomType { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public bool IsEmpty { get; set; }

        public Booking CurrentBooking { get; set; }


    }
}
