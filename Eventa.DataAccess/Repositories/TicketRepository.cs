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
    public class TicketRepository : ITicketRepository
    {
        private readonly EventaDbContext _context;

        public TicketRepository(EventaDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetByIdAsync(int ticketId)
        {
            return await _context.Tickets
                .Include(t => t.Bookings)
                .FirstOrDefaultAsync(t => t.TicketID == ticketId);
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByEventIdAsync(int eventId)
        {
            return await _context.Tickets
                .Where(t => t.EventID == eventId)
                .ToListAsync();
        }

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int ticketId)
        {
            return await _context.Tickets.AnyAsync(t => t.TicketID == ticketId);
        }

        public async Task<bool> HasAvailableQuantityAsync(int ticketId, int requestedQuantity)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            return ticket != null && ticket.AvailableQuantity >= requestedQuantity;
        }
    }
}