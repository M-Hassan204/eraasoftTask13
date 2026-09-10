using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eraasoftTask13.Data;
using eraasoftTask13.Models;
using eraasoftTask13.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace eraasoftTask13.Controllers
{
    public class HomeController : Controller
    {
        private readonly CinemaDbContext _context;

        public HomeController(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel
            {
                TotalMovies = await _context.Movies.CountAsync(),
                NowShowingMovies = await _context.Movies.CountAsync(m => m.Status == MovieStatus.NowShowing),
                TotalCinemas = await _context.Cinemas.CountAsync(),
                TotalActors = await _context.Actors.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalHalls = await _context.Halls.CountAsync(),
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalRevenue = await _context.Bookings.Where(b => b.Status == BookingStatus.Confirmed).SumAsync(b => b.TotalPrice),

                RecentBookings = await _context.Bookings
                    .Include(b => b.ShowTime).ThenInclude(s => s.Movie)
                    .Include(b => b.ShowTime).ThenInclude(s => s.Cinema)
                    .OrderByDescending(b => b.BookingDate)
                    .Take(10)
                    .ToListAsync(),

                UpcomingShows = await _context.ShowTimes
                    .Include(s => s.Movie)
                    .Include(s => s.Cinema)
                    .Include(s => s.Hall)
                    .Where(s => s.StartTime > System.DateTime.Now)
                    .OrderBy(s => s.StartTime)
                    .Take(10)
                    .ToListAsync(),

                PopularMovies = await _context.Movies
                    .Include(m => m.Category)
                    .Take(5)
                    .ToListAsync() // We will update this logic when we have real bookings
            };

            return View(vm);
        }
    }
}
