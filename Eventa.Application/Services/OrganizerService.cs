using Eventa.Application.DTOs.Event;
using Eventa.Application.DTOs.Organizer;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eventa.Application.Services
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IUnitOfWork _unit;

        public OrganizerService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<OrganizerEventsSummaryDto> GetOrganizerEventsSummaryAsync(string organizerId)
        {
            var events = await _unit.Events.GetAllQueryable()
                .Where(e => e.OrganizerId == organizerId)
                .ToListAsync();

            var ratings = await _unit.OrganizerRating.GetAllQueryable()
                .Where(r => r.OrganizerId == organizerId)
                .ToListAsync();

            double totalAverageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;
            int totalRatingsCount = ratings.Count;
            int totalEventsHosted = events.Count;  

            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                Location = e.Location,
                City = e.City,
                CategoryId = e.CategoryId,
                ImageUrl = e.ImageUrl,
                Status = e.Status,
                StartDateTime = e.StartDateTime,
                EndDateTime = e.EndDateTime
            }).ToList();

            return new OrganizerEventsSummaryDto
            {
                Events = eventDtos,
                TotalAverageRating = totalAverageRating,
                TotalRatingsCount = totalRatingsCount,
                TotalEventsHosted = totalEventsHosted 
            };
        }


        public async Task<bool> RateOrganizerAsync(string userId, string organizerId, int rating)
        {
            var user = await _unit.UserManager.FindByIdAsync(userId);
            var organizer = await _unit.UserManager.FindByIdAsync(organizerId);

            if (user == null || organizer == null) return false;

            var existing = await _unit.OrganizerRating.GetAllQueryable()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.OrganizerId == organizerId);

            if (existing != null)
            {
                existing.Rating = rating;
            }
            else
            {
                await _unit.OrganizerRating.CreateAsync(new OrganizerRating
                {
                    UserId = userId,
                    OrganizerId = organizerId,
                    Rating = rating,

                });
            }

            await _unit.SaveChangesAsync();
            return true;
        }
    }
}
