using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eraasoftTask13.Data;
using eraasoftTask13.Models;
using eraasoftTask13.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace eraasoftTask13.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly CinemaDbContext _context;

        public StatisticsController(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;

            var vm = new StatisticsViewModel
            {
                TotalMovies = await _context.Movies.CountAsync(),
                NowShowingMovies = await _context.Movies.CountAsync(m => m.Status == MovieStatus.NowShowing),
                ComingSoonMovies = await _context.Movies.CountAsync(m => m.Status == MovieStatus.ComingSoon),
                EndedMovies = await _context.Movies.CountAsync(m => m.Status == MovieStatus.Ended),

                TotalBookings = await _context.Bookings.CountAsync(),
                ConfirmedBookings = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Confirmed),
                PendingBookings = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Pending),
                CancelledBookings = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Cancelled),

                TotalRevenue = await _context.Bookings.Where(b => b.Status == BookingStatus.Confirmed).SumAsync(b => b.TotalPrice),
                RevenueToday = await _context.Bookings.Where(b => b.Status == BookingStatus.Confirmed && b.BookingDate.Date == now.Date).SumAsync(b => b.TotalPrice),
                RevenueThisMonth = await _context.Bookings.Where(b => b.Status == BookingStatus.Confirmed && b.BookingDate.Month == now.Month && b.BookingDate.Year == now.Year).SumAsync(b => b.TotalPrice)
            };

            var cinemas = await _context.Cinemas
                .Include(c => c.Movies)
                .Include(c => c.ShowTimes).ThenInclude(s => s.Bookings)
                .ToListAsync();

            foreach (var c in cinemas)
            {
                vm.MoviesPerCinema.Add(c.Name, c.Movies.Count);
                vm.BookingsPerCinema.Add(c.Name, c.ShowTimes.SelectMany(s => s.Bookings).Count());
                vm.RevenuePerCinema.Add(c.Name, c.ShowTimes.SelectMany(s => s.Bookings).Where(b => b.Status == BookingStatus.Confirmed).Sum(b => b.TotalPrice));
            }

            var categories = await _context.Categories.Include(c => c.Movies).ToListAsync();
            foreach (var c in categories)
            {
                vm.MoviesPerCategory.Add(c.Name, c.Movies.Count);
            }

            return View(vm);
        }
    }
}
