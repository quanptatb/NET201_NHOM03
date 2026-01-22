using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanThucAnNhanh.Data;
using WebBanThucAnNhanh.Models;

namespace WebBanThucAnNhanh.Controllers
{
    public class ThemeController : Controller
    {
        private readonly AppDbContext _context;

        public ThemeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Theme
        public async Task<IActionResult> Index()
        {
            return View(await _context.Themes.ToListAsync());
        }

        // GET: Theme/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes
                .FirstOrDefaultAsync(m => m.IdTheme == id);
            if (theme == null)
            {
                return NotFound();
            }

            return View(theme);
        }

        // GET: Theme/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Theme/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTheme,NameTheme")] Theme theme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theme);
        }

        // GET: Theme/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }
            return View(theme);
        }

        // POST: Theme/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTheme,NameTheme")] Theme theme)
        {
            if (id != theme.IdTheme)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThemeExists(theme.IdTheme))
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
            return View(theme);
        }

        // GET: Theme/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes
                .FirstOrDefaultAsync(m => m.IdTheme == id);
            if (theme == null)
            {
                return NotFound();
            }

            return View(theme);
        }

        // POST: Theme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme != null)
            {
                _context.Themes.Remove(theme);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThemeExists(int id)
        {
            return _context.Themes.Any(e => e.IdTheme == id);
        }
    }
}
