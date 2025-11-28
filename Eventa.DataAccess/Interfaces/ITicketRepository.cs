using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;

namespace Eventa.DataAccess.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(int ticketId);
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByEventIdAsync(int eventId);
        Task<Ticket> AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int ticketId);
        Task<bool> ExistsAsync(int ticketId);
        Task<bool> HasAvailableQuantityAsync(int ticketId, int requestedQuantity);
        Task DeleteAsync(Ticket ticket);
    }
}