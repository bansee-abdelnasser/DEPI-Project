using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Entities
{
    public class UserFavorite
    {
        public int Id { get; set; }

        public DateTime FavoritedAt { get; set; } = DateTime.UtcNow;

        public AppUser User { get; set; }
        public Event Event { get; set; }
    }

}
