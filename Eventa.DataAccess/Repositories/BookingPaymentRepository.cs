using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Repositories;

public class BookingPaymentRepository : IBookingPaymentRepository
{
    private readonly EventaDbContext _context;

    public BookingPaymentRepository(EventaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookingPayment>> GetAllAsync()
    {
        return await _context.BookingPayments.ToListAsync();
    }

    public async Task<BookingPayment?> GetByIdAsync(int id)
    {
        return await _context.BookingPayments.FindAsync(id);
    }

    public async Task AddAsync(BookingPayment bookingPayment)
    {
        _context.BookingPayments.Add(bookingPayment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BookingPayment bookingPayment)
    {
        _context.Entry(bookingPayment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(BookingPayment bookingPayment)
    {
        _context.BookingPayments.Remove(bookingPayment);
        await _context.SaveChangesAsync();
    }
}