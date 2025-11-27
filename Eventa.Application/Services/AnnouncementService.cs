using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.Application.DTOs.Announcement;
using Eventa.Application.Interfaces;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using AutoMapper;

namespace Eventa.Application.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public AnnouncementService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public IEnumerable<AnnouncementDto> GetAll()
        {
            var anns = _unit.Announcements.GetAll();
            return anns.Select(a => _mapper.Map<AnnouncementDto>(a));
        }

        public AnnouncementDto? GetById(int id)
        {
            var a = _unit.Announcements.FindById(id);
            if (a == null) return null;
            return _mapper.Map<AnnouncementDto>(a);
        }

        public AnnouncementDto Create(CreateAnnouncementDto dto)
        {
            var entity = _mapper.Map<Announcement>(dto);
            _unit.Announcements.Create(entity);
            _unit.Save();
            return _mapper.Map<AnnouncementDto>(entity);
        }

        public bool Update(int id, UpdateAnnouncementDto dto)
        {
            var existing = _unit.Announcements.FindById(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);

            _unit.Announcements.Update(existing);
            _unit.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var deleted = _unit.Announcements.Delete(id);
            if (deleted == null) return false;

            _unit.Save();
            return true;
        }
    }
}
