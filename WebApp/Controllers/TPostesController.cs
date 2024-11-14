using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TPostesController : Controller
    {
        private readonly WebAppContext _context;

        public TPostesController(WebAppContext context)
        {
            _context = context;
        }

        // GET: TPostes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TPostes.ToListAsync());
        }

        // GET: TPostes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPoste = await _context.TPostes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tPoste == null)
            {
                return NotFound();
            }

            return View(tPoste);
        }

        // GET: TPostes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TPostes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ResponsabilityRank")] TPoste tPoste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tPoste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tPoste);
        }

        // GET: TPostes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPoste = await _context.TPostes.FindAsync(id);
            if (tPoste == null)
            {
                return NotFound();
            }
            return View(tPoste);
        }

        // POST: TPostes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ResponsabilityRank")] TPoste tPoste)
        {
            if (id != tPoste.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tPoste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TPosteExists(tPoste.Id))
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
            return View(tPoste);
        }

        // GET: TPostes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPoste = await _context.TPostes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tPoste == null)
            {
                return NotFound();
            }

            return View(tPoste);
        }

        // POST: TPostes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tPoste = await _context.TPostes.FindAsync(id);
            if (tPoste != null)
            {
                _context.TPostes.Remove(tPoste);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TPosteExists(int id)
        {
            return _context.TPostes.Any(e => e.Id == id);
        }
    }
}
