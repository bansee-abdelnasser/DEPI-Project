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

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        => await _paymentRepository.GetAllAsync();

    public async Task<Payment?> GetPaymentByIdAsync(int id)
        => await _paymentRepository.GetByIdAsync(id);

    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        await _paymentRepository.AddAsync(payment);
        return payment;
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        await _paymentRepository.UpdateAsync(payment);
    }

    public async Task DeletePaymentAsync(int id)
    {
        var item = await _paymentRepository.GetByIdAsync(id);
        if (item != null)
            await _paymentRepository.DeleteAsync(id);
    }
}
