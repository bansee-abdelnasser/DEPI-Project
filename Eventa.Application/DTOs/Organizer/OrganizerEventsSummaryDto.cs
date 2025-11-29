using Eventa.Application.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.Application.DTOs.Organizer
{
    public class OrganizerEventsSummaryDto
    {
        public IEnumerable<EventDto> Events { get; set; } = new List<EventDto>();
        public double TotalAverageRating { get; set; }
        public int TotalRatingsCount { get; set; }
        public int TotalEventsHosted { get; set; }

    }
}