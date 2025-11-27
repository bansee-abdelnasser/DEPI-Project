using System.Collections.Generic;
using Eventa.Application.DTOs.Announcement;

namespace Eventa.Application.Interfaces
{
    public interface IAnnouncementService
    {
        IEnumerable<AnnouncementDto> GetAll();
        AnnouncementDto? GetById(int id);
        AnnouncementDto Create(CreateAnnouncementDto dto);
        bool Update(int id, UpdateAnnouncementDto dto);
        bool Delete(int id);
    }
}
