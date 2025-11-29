using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string? TransactionID { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<BookingPayment> BookingPayments { get; set; } = new List<BookingPayment>();
    }
}