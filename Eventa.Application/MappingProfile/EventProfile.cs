using AutoMapper;
using Eventa.DataAccess.Entities;
using Eventa.Application.DTOs.Event;

namespace Eventa.Application.MappingProfiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            // Entity → DTO
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // Create DTO → Entity
            CreateMap<CreateEventDto, Event>();

            // Update DTO → Entity
            CreateMap<UpdateEventDto, Event>();
        }
    }
}
