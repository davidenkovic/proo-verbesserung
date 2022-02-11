using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class BookingForCustomerDTO
    {
        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public string RoomNumber { get; set; }

        public string RoomType { get; set; }

        public double DaysSleeped { get; set; }
    }
}
