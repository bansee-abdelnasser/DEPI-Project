using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.Entities;

namespace Eventa.DataAccess.Interfaces
{
    public interface IAnnouncementRepository : IBaseRepository<Announcement, int>
    {
    }
}
