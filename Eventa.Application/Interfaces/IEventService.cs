using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;
using Eventa.Application.DTOs.Event;

namespace Eventa.Application.Interfaces
{
    public interface IEventService
    {
        IEnumerable<EventDto> GetAllEvents();
        EventDto? GetEventById(int id);
        EventDto CreateEvent(CreateEventDto dto);
        bool UpdateEvent(int id, UpdateEventDto dto);
        bool DeleteEvent(int id);
    }
}

