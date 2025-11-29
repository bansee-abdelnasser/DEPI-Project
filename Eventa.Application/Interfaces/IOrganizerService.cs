using Eventa.Application.DTOs.Organizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.Application.Interfaces
{
    public interface IOrganizerService
    {
        Task<OrganizerEventsSummaryDto> GetOrganizerEventsSummaryAsync(string organizerId);
        Task<bool> RateOrganizerAsync(string userId, string organizerId, int rating);
    }
}
