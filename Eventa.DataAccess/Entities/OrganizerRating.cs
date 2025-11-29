using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Entities
{
    public class OrganizerRating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } 

        [Required]
        public string OrganizerId { get; set; } 

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } 

        public DateTime RatedAt { get; set; } = DateTime.UtcNow;

    }
}
