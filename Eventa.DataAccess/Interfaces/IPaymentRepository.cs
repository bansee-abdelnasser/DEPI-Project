using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;

namespace Eventa.DataAccess.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int paymentId);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByTransactionIdAsync(string transactionId);
        Task<Payment> AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(int paymentId);
        Task<bool> ExistsAsync(int paymentId);
        Task DeleteAsync(Payment payment);
    }
}