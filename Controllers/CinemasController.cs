using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eraasoftTask13.Data;
using eraasoftTask13.Models;
using eraasoftTask13.Services;
using eraasoftTask13.ViewModels;

namespace eraasoftTask13.Controllers
{
    public class CinemasController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly FileService _fileService;

        public CinemasController(CinemaDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: Cinemas
        public async Task<IActionResult> Index()
        {
            var cinemas = await _context.Cinemas
                .Include(c => c.Halls)
                .Include(c => c.Movies)
                .ToListAsync();
            return View(cinemas);
        }

        // GET: Cinemas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cinema = await _context.Cinemas
                .Include(c => c.Halls)
                .Include(c => c.Movies).ThenInclude(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cinema == null) return NotFound();
            return View(cinema);
        }

        // GET: Cinemas/Create
        public IActionResult Create()
        {
            return View(new CinemaCreateViewModel());
        }

        // POST: Cinemas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CinemaCreateViewModel vm)
        {
            var imgError = _fileService.Validate(vm.ImageFile);
            if (imgError != null) ModelState.AddModelError("ImageFile", imgError);

            if (!ModelState.IsValid) return View(vm);

            var imagePath = await _fileService.SaveFileAsync(vm.ImageFile, "cinemas");

            var cinema = new Cinema
            {
                Name = vm.Name,
                Description = vm.Description,
                Address = vm.Address,
                City = vm.City,
                Phone = vm.Phone,
                Email = vm.Email,
                Image = imagePath ?? "",
                IsActive = vm.IsActive,
                CreatedAt = System.DateTime.UtcNow
            };

            _context.Add(cinema);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cinema created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Cinemas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null) return NotFound();

            var vm = new CinemaEditViewModel
            {
                Id = cinema.Id,
                Name = cinema.Name,
                Description = cinema.Description,
                Address = cinema.Address,
                City = cinema.City,
                Phone = cinema.Phone,
                Email = cinema.Email,
                IsActive = cinema.IsActive,
                ExistingImage = cinema.Image
            };

            return View(vm);
        }

        // POST: Cinemas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CinemaEditViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            var imgError = _fileService.Validate(vm.ImageFile);
            if (imgError != null) ModelState.AddModelError("ImageFile", imgError);

            if (!ModelState.IsValid) return View(vm);

            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null) return NotFound();

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                _fileService.DeleteFile(cinema.Image);
                cinema.Image = await _fileService.SaveFileAsync(vm.ImageFile, "cinemas") ?? cinema.Image;
            }

            cinema.Name = vm.Name;
            cinema.Description = vm.Description;
            cinema.Address = vm.Address;
            cinema.City = vm.City;
            cinema.Phone = vm.Phone;
            cinema.Email = vm.Email;
            cinema.IsActive = vm.IsActive;

            try
            {
                _context.Update(cinema);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cinemas.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            TempData["Success"] = "Cinema updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Cinemas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cinema = await _context.Cinemas.FirstOrDefaultAsync(m => m.Id == id);
            if (cinema == null) return NotFound();

            return View(cinema);
        }

        // POST: Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                _fileService.DeleteFile(cinema.Image);
                _context.Cinemas.Remove(cinema);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cinema deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
