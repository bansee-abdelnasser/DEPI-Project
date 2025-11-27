using AutoMapper;
using Eventa.DataAccess.Entities;
using Eventa.Application.DTOs.Announcement;

namespace Eventa.Application.MappingProfiles
{
    public class AnnouncementProfile : Profile
    {
        public AnnouncementProfile()
        {
            // Entity → DTO
            CreateMap<Announcement, AnnouncementDto>();

            // Create DTO → Entity
            CreateMap<CreateAnnouncementDto, Announcement>();

            // Update DTO → Entity
            CreateMap<UpdateAnnouncementDto, Announcement>();
        }
    }
}
