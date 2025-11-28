using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventa.Application.Services;

public interface IBookingPaymentService
{
    Task<IEnumerable<BookingPayment>> GetAllAsync();
    Task<BookingPayment?> GetByIdAsync(int id);
    Task<BookingPayment> CreateAsync(BookingPayment bookingPayment);
    Task UpdateAsync(BookingPayment bookingPayment);
    Task DeleteAsync(int id);
}