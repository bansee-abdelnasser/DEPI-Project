using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Entities
{
     public class BookingPayment
    {
     
            public int BookingPaymentID { get; set; }
            public int BookingID { get; set; }
            public int PaymentID { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public virtual Booking? Booking { get; set; }
            public virtual Payment? Payment { get; set; }
        
    }

}
