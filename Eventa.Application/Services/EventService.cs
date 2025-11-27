using Eventa.Application.Interfaces;
using Eventa.Application.DTOs.Event;
using AutoMapper;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
namespace Eventa.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public IEnumerable<EventDto> GetAllEvents()
        {
            var events = _unit.Events.GetAll();
            return events.Select(e => _mapper.Map<EventDto>(e));
        }

        public EventDto? GetEventById(int id)
        {
            var e = _unit.Events.FindById(id);
            if (e == null) return null;
            return _mapper.Map<EventDto>(e);
        }

        public EventDto CreateEvent(CreateEventDto dto)
        {
            var entity = _mapper.Map<Event>(dto);
            _unit.Events.Create(entity);
            _unit.Save();
            return _mapper.Map<EventDto>(entity);
        }

        public bool UpdateEvent(int id, UpdateEventDto dto)
        {
            var existing = _unit.Events.FindById(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing); // map fields into existing entity
            _unit.Events.Update(existing);
            _unit.Save();
            return true;
        }

        public bool DeleteEvent(int id)
        {
            var deleted = _unit.Events.Delete(id);
            if (deleted == null) return false;
            _unit.Save();
            return true;
        }
    }
}
