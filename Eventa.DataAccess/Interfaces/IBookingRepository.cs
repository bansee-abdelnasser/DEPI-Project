using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;

namespace Eventa.DataAccess.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetByEventIdAsync(int eventId);
        Task<Booking> AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int bookingId);
        Task<bool> ExistsAsync(int bookingId);
    }
}