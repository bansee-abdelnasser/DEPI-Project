using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace Eventa.Application.DTOs.Event
{
    public class UpdateEventDto
    {


        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        public string? Location { get; set; }
        public string? City { get; set; }
        public string? Recurrence { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        public string? Status { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
