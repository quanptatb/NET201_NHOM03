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
    public class FastFoodController : Controller
    {
        private readonly AppDbContext _context;

        public FastFoodController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FastFood
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FastFoods.Include(f => f.TypeOfFastFood);
            return View(await appDbContext.ToListAsync());
        }

        // GET: FastFood/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fastFood = await _context.FastFoods
                .Include(f => f.TypeOfFastFood)
                .FirstOrDefaultAsync(m => m.IdFastFood == id);
            if (fastFood == null)
            {
                return NotFound();
            }

            return View(fastFood);
        }

        // GET: FastFood/Create
        public IActionResult Create()
        {
            ViewData["IdTypeOfFastFood"] = new SelectList(_context.TypeOfFastFoods, "IdTypeOfFastFood", "NameTypeOfFastFood");
            return View();
        }

        // POST: FastFood/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFastFood,NameFastFood,Price,Quantity,Image,Status,Description,IdTypeOfFastFood")] FastFood fastFood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fastFood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTypeOfFastFood"] = new SelectList(_context.TypeOfFastFoods, "IdTypeOfFastFood", "NameTypeOfFastFood", fastFood.IdTypeOfFastFood);
            return View(fastFood);
        }

        // GET: FastFood/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fastFood = await _context.FastFoods.FindAsync(id);
            if (fastFood == null)
            {
                return NotFound();
            }
            ViewData["IdTypeOfFastFood"] = new SelectList(_context.TypeOfFastFoods, "IdTypeOfFastFood", "NameTypeOfFastFood", fastFood.IdTypeOfFastFood);
            return View(fastFood);
        }

        // POST: FastFood/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFastFood,NameFastFood,Price,Quantity,Image,Status,Description,IdTypeOfFastFood")] FastFood fastFood)
        {
            if (id != fastFood.IdFastFood)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fastFood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FastFoodExists(fastFood.IdFastFood))
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
            ViewData["IdTypeOfFastFood"] = new SelectList(_context.TypeOfFastFoods, "IdTypeOfFastFood", "NameTypeOfFastFood", fastFood.IdTypeOfFastFood);
            return View(fastFood);
        }

        // GET: FastFood/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fastFood = await _context.FastFoods
                .Include(f => f.TypeOfFastFood)
                .FirstOrDefaultAsync(m => m.IdFastFood == id);
            if (fastFood == null)
            {
                return NotFound();
            }

            return View(fastFood);
        }

        // POST: FastFood/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fastFood = await _context.FastFoods.FindAsync(id);
            if (fastFood != null)
            {
                _context.FastFoods.Remove(fastFood);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FastFoodExists(int id)
        {
            return _context.FastFoods.Any(e => e.IdFastFood == id);
        }
    }
}
