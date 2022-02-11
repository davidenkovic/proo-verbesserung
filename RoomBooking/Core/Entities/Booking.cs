using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Booking : EntityObject
    {
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room? Room { get; set; }
        public int RoomId { get; set; }

        public DateTime From { get; set; }
        public DateTime? To { get; set; }

        public override string ToString()
        {
            return $"{Room} From {From.ToShortDateString()} To: {To?.ToShortDateString()}";
        }
    }
}
