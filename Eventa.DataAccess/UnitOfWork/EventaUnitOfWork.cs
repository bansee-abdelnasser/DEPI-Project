using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Eventa.DataAccess.Repositories;
using Eventa.DataAccess.Repositories.Todo.DataAccess.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.UnitOfWork
{
    public class EventaUnitOfWork : IUnitOfWork
    {
        private readonly EventaDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenManager _tokenManager;


        private readonly Lazy<IFavoriteRepository> _favorites;

        private readonly Lazy<IBookingRepository> _bookings;
        private readonly Lazy<ITicketRepository> _tickets;
        private readonly Lazy<IPaymentRepository> _payments;
        private readonly Lazy<IBookingPaymentRepository> _bookingPayments;

        public EventaUnitOfWork(EventaDbContext context, UserManager<AppUser> userManger, ITokenManager tokenManager, IEventRepository events,
         ICategoryRepository categories,
         IAnnouncementRepository announcements,
            IBookingRepository bookings,
        ITicketRepository tickets,
        IPaymentRepository payments,
        IBookingPaymentRepository bookingPayments)
        {
            _context = context;
            _userManager = userManger;
            _tokenManager = tokenManager;
            _favorites = new Lazy<IFavoriteRepository>(() => new FavoriteRepository(_context));

            Events = events;
            Categories = categories;
            Announcements = announcements;
        }

        public UserManager<AppUser> UserManager => _userManager;
        public ITokenManager TokenManager => _tokenManager;

        
        public IFavoriteRepository Favorites => _favorites.Value;


        //public void SaveChanges();
        public ICategoryRepository Categories { get; }
        public IEventRepository Events { get; }
        public IAnnouncementRepository Announcements { get; }

        public IBookingRepository Bookings { get; }
        public ITicketRepository Tickets { get; }
        public IPaymentRepository Payments { get; }
        public IBookingPaymentRepository BookingPayments { get; }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
