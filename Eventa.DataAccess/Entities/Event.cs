using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Eventa.DataAccess.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? City { get; set; }

        public string? Recurrence { get; set; }

        //  Category Relation
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        //  Organizer Relation
        public string? OrganizerId { get; set; }
        public AppUser? Organizer { get; set; }


        //  Announcements (1 Event -> many Announcements)
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

        public string? ImageUrl { get; set; }

        public string? Status { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

      
    }
}
