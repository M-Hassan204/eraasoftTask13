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
    public class ActorsController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly FileService _fileService;

        public ActorsController(CinemaDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            var actors = await _context.Actors
                .Select(a => new { a.Id, a.Name, a.Bio, a.Image, a.IsActive,
                    MovieCount = a.MovieActors.Count })
                .ToListAsync();

            // Re-query to get full Actor objects with movie counts in ViewBag
            var actorList = await _context.Actors
                .Include(a => a.MovieActors)
                .ToListAsync();

            return View(actorList);
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var actor = await _context.Actors
                .Include(a => a.MovieActors).ThenInclude(ma => ma.Movie).ThenInclude(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (actor == null) return NotFound();
            return View(actor);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View(new ActorCreateViewModel());
        }

        // POST: Actors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActorCreateViewModel vm)
        {
            // Validate image
            var imgError = _fileService.Validate(vm.ImageFile);
            if (imgError != null) ModelState.AddModelError("ImageFile", imgError);

            if (!ModelState.IsValid) return View(vm);

            var imagePath = await _fileService.SaveFileAsync(vm.ImageFile, "actors");

            var actor = new Actor
            {
                Name = vm.Name,
                Bio = vm.Bio,
                Image = imagePath ?? "",
                IsActive = vm.IsActive,
                CreatedAt = System.DateTime.UtcNow
            };

            _context.Add(actor);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Actor created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();

            var vm = new ActorEditViewModel
            {
                Id = actor.Id,
                Name = actor.Name,
                Bio = actor.Bio,
                IsActive = actor.IsActive,
                ExistingImage = actor.Image
            };

            return View(vm);
        }

        // POST: Actors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActorEditViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            var imgError = _fileService.Validate(vm.ImageFile);
            if (imgError != null) ModelState.AddModelError("ImageFile", imgError);

            if (!ModelState.IsValid) return View(vm);

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();

            // Handle image replacement
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                _fileService.DeleteFile(actor.Image);
                actor.Image = await _fileService.SaveFileAsync(vm.ImageFile, "actors") ?? actor.Image;
            }

            actor.Name = vm.Name;
            actor.Bio = vm.Bio;
            actor.IsActive = vm.IsActive;

            try
            {
                _context.Update(actor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Actors.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            TempData["Success"] = "Actor updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null) return NotFound();

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                _fileService.DeleteFile(actor.Image);
                _context.Actors.Remove(actor);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Actor deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
