using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Interfaces;

public interface IBookingPaymentRepository
{
    Task<IEnumerable<BookingPayment>> GetAllAsync();
    Task<BookingPayment?> GetByIdAsync(int id);
    Task AddAsync(BookingPayment entity);
    Task UpdateAsync(BookingPayment entity);
    Task DeleteAsync(BookingPayment entity);
}