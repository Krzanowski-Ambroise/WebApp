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
    public class TSitesController : Controller
    {
        private readonly WebAppContext _context;

        public TSitesController(WebAppContext context)
        {
            _context = context;
        }

        // GET: TSites
        public async Task<IActionResult> Index()
        {
            var webAppContext = _context.TSites.Include(t => t.Companie);
            return View(await webAppContext.ToListAsync());
        }

        // GET: TSites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tSite = await _context.TSites
                .Include(t => t.Companie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tSite == null)
            {
                return NotFound();
            }

            return View(tSite);
        }

        // GET: TSites/Create
        public IActionResult Create()
        {
            ViewData["CompanieId"] = new SelectList(_context.TCompanies, "Id", "Id");
            return View();
        }

        // POST: TSites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanieId,Title,Street,City,Postcode")] TSite tSite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tSite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanieId"] = new SelectList(_context.TCompanies, "Id", "Id", tSite.CompanieId);
            return View(tSite);
        }

        // GET: TSites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tSite = await _context.TSites.FindAsync(id);
            if (tSite == null)
            {
                return NotFound();
            }
            ViewData["CompanieId"] = new SelectList(_context.TCompanies, "Id", "Id", tSite.CompanieId);
            return View(tSite);
        }

        // POST: TSites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanieId,Title,Street,City,Postcode")] TSite tSite)
        {
            if (id != tSite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tSite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TSiteExists(tSite.Id))
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
            ViewData["CompanieId"] = new SelectList(_context.TCompanies, "Id", "Id", tSite.CompanieId);
            return View(tSite);
        }

        // GET: TSites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tSite = await _context.TSites
                .Include(t => t.Companie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tSite == null)
            {
                return NotFound();
            }

            return View(tSite);
        }

        // POST: TSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tSite = await _context.TSites.FindAsync(id);
            if (tSite != null)
            {
                _context.TSites.Remove(tSite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TSiteExists(int id)
        {
            return _context.TSites.Any(e => e.Id == id);
        }
    }
}
