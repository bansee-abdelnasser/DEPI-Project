using Eventa.Application.DTOs.User;
using Eventa.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Event>> GetUserFavoritesAsync(string userId);
        Task<bool?> ToggleFavoriteAsync(int eventId,string userId);
    }
}
