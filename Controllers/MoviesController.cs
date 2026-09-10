using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eraasoftTask13.Data;
using eraasoftTask13.Models;
using eraasoftTask13.Services;
using eraasoftTask13.ViewModels;

namespace eraasoftTask13.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly FileService _fileService;

        public MoviesController(CinemaDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.MovieActors)
                .ToListAsync();
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.MovieImages)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.ShowTimes).ThenInclude(s => s.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();
            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            ViewData["Actors"] = _context.Actors.Where(a => a.IsActive).ToList();
            return View(new MovieCreateViewModel());
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieCreateViewModel vm)
        {
            // Validate files
            var mainErr = _fileService.Validate(vm.MainImageFile);
            if (mainErr != null) ModelState.AddModelError("MainImageFile", mainErr);

            if (vm.SubImageFiles != null)
            {
                foreach (var sub in vm.SubImageFiles)
                {
                    var subErr = _fileService.Validate(sub);
                    if (subErr != null)
                    {
                        ModelState.AddModelError("SubImageFiles", subErr);
                        break;
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", vm.CategoryId);
                ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", vm.CinemaId);
                ViewData["Actors"] = _context.Actors.Where(a => a.IsActive).ToList();
                return View(vm);
            }

            var mainImagePath = await _fileService.SaveFileAsync(vm.MainImageFile, "movies");

            var movie = new Movie
            {
                Name = vm.Name,
                Description = vm.Description,
                Status = vm.Status,
                ReleaseDate = vm.ReleaseDate,
                DurationInMinutes = vm.DurationInMinutes,
                MainImage = mainImagePath ?? "",
                CategoryId = vm.CategoryId,
                CinemaId = vm.CinemaId,
                IsActive = vm.IsActive,
                CreatedAt = System.DateTime.UtcNow
            };

            _context.Add(movie);
            await _context.SaveChangesAsync();

            // Save actor relationships
            if (vm.SelectedActorIds != null && vm.SelectedActorIds.Count > 0)
            {
                foreach (var actorId in vm.SelectedActorIds)
                {
                    _context.MovieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actorId });
                }
            }

            // Save sub images
            if (vm.SubImageFiles != null)
            {
                int order = 1;
                foreach (var subFile in vm.SubImageFiles)
                {
                    var subPath = await _fileService.SaveFileAsync(subFile, "movies");
                    if (subPath != null)
                    {
                        _context.MovieImages.Add(new MovieImage
                        {
                            MovieId = movie.Id,
                            ImagePath = subPath,
                            IsMain = false,
                            DisplayOrder = order++
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Movie created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.MovieImages)
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            var vm = new MovieEditViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Status = movie.Status,
                ReleaseDate = movie.ReleaseDate,
                DurationInMinutes = movie.DurationInMinutes,
                IsActive = movie.IsActive,
                CategoryId = movie.CategoryId,
                CinemaId = movie.CinemaId,
                SelectedActorIds = movie.MovieActors.Select(ma => ma.ActorId).ToList(),
                ExistingMainImage = movie.MainImage,
                ExistingSubImages = movie.MovieImages.Where(i => !i.IsMain).OrderBy(i => i.DisplayOrder).ToList()
            };

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", vm.CategoryId);
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", vm.CinemaId);
            ViewData["Actors"] = _context.Actors.Where(a => a.IsActive).ToList();
            return View(vm);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieEditViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            var mainErr = _fileService.Validate(vm.MainImageFile);
            if (mainErr != null) ModelState.AddModelError("MainImageFile", mainErr);

            if (vm.SubImageFiles != null)
            {
                foreach (var sub in vm.SubImageFiles)
                {
                    var subErr = _fileService.Validate(sub);
                    if (subErr != null)
                    {
                        ModelState.AddModelError("SubImageFiles", subErr);
                        break;
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                // Repopulate existing images for the view
                vm.ExistingSubImages = await _context.MovieImages
                    .Where(i => i.MovieId == id && !i.IsMain)
                    .OrderBy(i => i.DisplayOrder)
                    .ToListAsync();
                vm.ExistingMainImage = (await _context.Movies.FindAsync(id))?.MainImage;

                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", vm.CategoryId);
                ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", vm.CinemaId);
                ViewData["Actors"] = _context.Actors.Where(a => a.IsActive).ToList();
                return View(vm);
            }

            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .Include(m => m.MovieImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            // Replace main image if new one uploaded
            if (vm.MainImageFile != null && vm.MainImageFile.Length > 0)
            {
                _fileService.DeleteFile(movie.MainImage);
                movie.MainImage = await _fileService.SaveFileAsync(vm.MainImageFile, "movies") ?? movie.MainImage;
            }

            // Delete selected sub images
            if (vm.DeletedSubImageIds != null && vm.DeletedSubImageIds.Count > 0)
            {
                var toDelete = movie.MovieImages.Where(i => vm.DeletedSubImageIds.Contains(i.Id)).ToList();
                foreach (var img in toDelete)
                {
                    _fileService.DeleteFile(img.ImagePath);
                    _context.MovieImages.Remove(img);
                }
            }

            // Add new sub images
            if (vm.SubImageFiles != null && vm.SubImageFiles.Count > 0)
            {
                int maxOrder = movie.MovieImages.Any() ? movie.MovieImages.Max(i => i.DisplayOrder) : 0;
                foreach (var subFile in vm.SubImageFiles)
                {
                    var subPath = await _fileService.SaveFileAsync(subFile, "movies");
                    if (subPath != null)
                    {
                        _context.MovieImages.Add(new MovieImage
                        {
                            MovieId = movie.Id,
                            ImagePath = subPath,
                            IsMain = false,
                            DisplayOrder = ++maxOrder
                        });
                    }
                }
            }

            // Update actor links
            _context.MovieActors.RemoveRange(movie.MovieActors);
            if (vm.SelectedActorIds != null && vm.SelectedActorIds.Count > 0)
            {
                foreach (var actorId in vm.SelectedActorIds)
                {
                    _context.MovieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actorId });
                }
            }

            movie.Name = vm.Name;
            movie.Description = vm.Description;
            movie.Status = vm.Status;
            movie.ReleaseDate = vm.ReleaseDate;
            movie.DurationInMinutes = vm.DurationInMinutes;
            movie.IsActive = vm.IsActive;
            movie.CategoryId = vm.CategoryId;
            movie.CinemaId = vm.CinemaId;

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movies.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            TempData["Success"] = "Movie updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie != null)
            {
                _fileService.DeleteFile(movie.MainImage);
                foreach (var img in movie.MovieImages)
                    _fileService.DeleteFile(img.ImagePath);
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Movie deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
