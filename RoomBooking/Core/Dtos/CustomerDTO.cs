using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class CustomerDTO
    {
        public string LastName { get; set; }    
        public string FirstName { get; set; }   
        public int CountBookings { get; set; }
        public int CustomerId { get; set; }
    }
}
