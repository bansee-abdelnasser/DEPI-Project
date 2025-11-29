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

public class BookingPaymentService : IBookingPaymentService
{
    private readonly IBookingPaymentRepository _bookingPaymentRepository;

    public BookingPaymentService(IBookingPaymentRepository bookingPaymentRepository)
    {
        _bookingPaymentRepository = bookingPaymentRepository;
    }

    public async Task<IEnumerable<BookingPayment>> GetAllAsync()
        => await _bookingPaymentRepository.GetAllAsync();

    public async Task<BookingPayment?> GetByIdAsync(int id)
        => await _bookingPaymentRepository.GetByIdAsync(id);

    public async Task<BookingPayment> CreateAsync(BookingPayment bookingPayment)
    {
        await _bookingPaymentRepository.AddAsync(bookingPayment);
        return bookingPayment;
    }

    public async Task UpdateAsync(BookingPayment bookingPayment)
    {
        await _bookingPaymentRepository.UpdateAsync(bookingPayment);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _bookingPaymentRepository.GetByIdAsync(id);
        if (item != null)
            await _bookingPaymentRepository.DeleteAsync(item);
    }
}