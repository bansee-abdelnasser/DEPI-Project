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

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        => await _bookingRepository.GetAllAsync();

    public async Task<Booking?> GetBookingByIdAsync(int id)
        => await _bookingRepository.GetByIdAsync(id);

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        await _bookingRepository.AddAsync(booking);
        return booking;
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        await _bookingRepository.UpdateAsync(booking);
    }

    public async Task DeleteBookingAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking != null)
            await _bookingRepository.DeleteAsync(booking);
    }
}