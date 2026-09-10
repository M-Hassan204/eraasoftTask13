using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eraasoftTask13.Data;
using eraasoftTask13.Models;
using eraasoftTask13.ViewModels;

namespace eraasoftTask13
{
    public class BookingsController : Controller
    {
        private readonly CinemaDbContext _context;

        public BookingsController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var cinemaDbContext = _context.Bookings.Include(b => b.ShowTime);
            return View(await cinemaDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.ShowTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["ShowTimeId"] = new SelectList(_context.ShowTimes, "Id", "Id");
            return View();
        }

        public async Task<IActionResult> Book(int showTimeId)
        {
            var showTime = await _context.ShowTimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.Id == showTimeId);

            if (showTime == null) return NotFound();

            var allSeats = await _context.Seats.Where(s => s.HallId == showTime.HallId).ToListAsync();
            var bookedSeatIds = await _context.BookingSeats
                .Include(bs => bs.Booking)
                .Where(bs => bs.Booking.ShowTimeId == showTimeId && bs.Booking.Status != BookingStatus.Cancelled)
                .Select(bs => bs.SeatId)
                .ToListAsync();

            var vm = new BookingViewModel
            {
                ShowTime = showTime,
                AllSeats = allSeats,
                BookedSeatIds = bookedSeatIds
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookingPostViewModel model)
        {
            var showTime = await _context.ShowTimes.FindAsync(model.ShowTimeId);
            if (showTime == null) return NotFound();

            if (model.SelectedSeatIds == null || !model.SelectedSeatIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one seat.");
                return RedirectToAction(nameof(Book), new { showTimeId = model.ShowTimeId });
            }

            // Check if seats are already booked
            var bookedSeats = await _context.BookingSeats
                .Include(bs => bs.Booking)
                .Where(bs => bs.Booking.ShowTimeId == model.ShowTimeId && bs.Booking.Status != BookingStatus.Cancelled)
                .Select(bs => bs.SeatId)
                .ToListAsync();

            if (model.SelectedSeatIds.Any(id => bookedSeats.Contains(id)))
            {
                ModelState.AddModelError("", "One or more selected seats are already booked.");
                return RedirectToAction(nameof(Book), new { showTimeId = model.ShowTimeId });
            }

            var booking = new Booking
            {
                ShowTimeId = model.ShowTimeId,
                CustomerName = model.CustomerName,
                CustomerEmail = model.CustomerEmail,
                CustomerPhone = model.CustomerPhone,
                BookingDate = DateTime.Now,
                Status = BookingStatus.Confirmed,
                BookingReference = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                TotalPrice = showTime.TicketPrice * model.SelectedSeatIds.Count
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(); // save to get booking Id

            foreach (var seatId in model.SelectedSeatIds)
            {
                _context.BookingSeats.Add(new BookingSeat
                {
                    BookingId = booking.Id,
                    SeatId = seatId,
                    Price = showTime.TicketPrice
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookingReference,CustomerName,CustomerEmail,CustomerPhone,ShowTimeId,TotalPrice,BookingDate,Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowTimeId"] = new SelectList(_context.ShowTimes, "Id", "Id", booking.ShowTimeId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["ShowTimeId"] = new SelectList(_context.ShowTimes, "Id", "Id", booking.ShowTimeId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookingReference,CustomerName,CustomerEmail,CustomerPhone,ShowTimeId,TotalPrice,BookingDate,Status")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowTimeId"] = new SelectList(_context.ShowTimes, "Id", "Id", booking.ShowTimeId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.ShowTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
