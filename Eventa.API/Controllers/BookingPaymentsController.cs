using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eventa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingPaymentsController : ControllerBase
{
    private readonly EventaDbContext _context;

    public BookingPaymentsController(EventaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingPayment>>> GetBookingPayments()
    {
        return await _context.BookingPayments.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingPayment>> GetBookingPayment(int id)
    {
        var bookingPayment = await _context.BookingPayments.FindAsync(id);

        if (bookingPayment == null)
        {
            return NotFound();
        }

        return bookingPayment;
    }

    [HttpPost]
    public async Task<ActionResult<BookingPayment>> CreateBookingPayment(BookingPayment bookingPayment)
    {
        _context.BookingPayments.Add(bookingPayment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBookingPayment), new { id = bookingPayment.BookingPaymentID }, bookingPayment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBookingPayment(int id, BookingPayment bookingPayment)
    {
        if (id != bookingPayment.BookingPaymentID)
        {
            return BadRequest();
        }

        _context.Entry(bookingPayment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingPaymentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBookingPayment(int id)
    {
        var bookingPayment = await _context.BookingPayments.FindAsync(id);
        if (bookingPayment == null)
        {
            return NotFound();
        }

        _context.BookingPayments.Remove(bookingPayment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BookingPaymentExists(int id)
    {
        return _context.BookingPayments.Any(e => e.BookingPaymentID == id);
    }
}