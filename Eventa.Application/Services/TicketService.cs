using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.Application.Services;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventa.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        => await _ticketRepository.GetAllAsync();

    public async Task<Ticket?> GetTicketByIdAsync(int id)
        => await _ticketRepository.GetByIdAsync(id);

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        await _ticketRepository.AddAsync(ticket);
        return ticket;
    }

    public async Task UpdateTicketAsync(Ticket ticket)
    {
        await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task DeleteTicketAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket != null)
            await _ticketRepository.DeleteAsync(ticket);
    }
}