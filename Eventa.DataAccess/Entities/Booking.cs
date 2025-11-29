using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Entities
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int UserID { get; set; }
        public int EventID { get; set; }
        public int TicketID { get; set; }
        public int NumberOfTickets { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }

        public virtual Ticket? Ticket { get; set; }
        public virtual ICollection<BookingPayment> BookingPayments { get; set; } = new List<BookingPayment>();
    }
}
