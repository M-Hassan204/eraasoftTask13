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
    public class CategoriesController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly FileService _fileService;

        public CategoriesController(CinemaDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(c => c.Movies)
                .ToListAsync();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .Include(c => c.Movies).ThenInclude(m => m.Cinema)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null) return NotFound();
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel vm)
        {
            var imgError = _fileService.Validate(vm.ImageFile);
            if (imgError != null) ModelState.AddModelError("ImageFile", imgError);

            if (!ModelState.IsValid) return View(vm);

            var imagePath = await _fileService.SaveFileAsync(vm.ImageFile, "categories");

            var category = new Category
            {
                Name = vm.Name,
                Description = vm.Description,
                Image = imagePath ?? "",
                IsActive = vm.IsActive,
                CreatedAt = System.DateTime.UtcNow
            };

            _context.Add(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var vm = new CategoryEditViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                ExistingImage = category.Image
            };

            return View(vm);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEditViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            var imgError = _fileService.Validate(vm.ImageFile);
            if (imgError != null) ModelState.AddModelError("ImageFile", imgError);

            if (!ModelState.IsValid) return View(vm);

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                _fileService.DeleteFile(category.Image);
                category.Image = await _fileService.SaveFileAsync(vm.ImageFile, "categories") ?? category.Image;
            }

            category.Name = vm.Name;
            category.Description = vm.Description;
            category.IsActive = vm.IsActive;

            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _fileService.DeleteFile(category.Image);
                _context.Categories.Remove(category);
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
