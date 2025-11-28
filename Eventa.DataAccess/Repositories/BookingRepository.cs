using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventa.DataAccess.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly EventaDbContext _context;

        public BookingRepository(EventaDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Ticket)
                .Include(b => b.BookingPayments)
                    .ThenInclude(bp => bp.Payment)
                .FirstOrDefaultAsync(b => b.BookingID == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Ticket)
                .Include(b => b.BookingPayments)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Ticket)
                .Where(b => b.UserID == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByEventIdAsync(int eventId)
        {
            return await _context.Bookings
                .Include(b => b.Ticket)
                .Where(b => b.EventID == eventId)
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int bookingId)
        {
            return await _context.Bookings.AnyAsync(b => b.BookingID == bookingId);
        }
    }
}
