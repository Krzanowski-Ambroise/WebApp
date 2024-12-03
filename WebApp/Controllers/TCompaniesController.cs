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
    public class TCompaniesController : Controller
    {
        private readonly WebAppContext _context;

        public TCompaniesController(WebAppContext context)
        {
            _context = context;
        }

        // GET: TCompanies
        public async Task<IActionResult> Index()
        {
            return View(await _context.TCompanies.ToListAsync());
        }
        public async Task<IActionResult> List()
        {
            return View(await _context.TCompanies.ToListAsync());
        }

        // GET: TCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tCompany = await _context.TCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tCompany == null)
            {
                return NotFound();
            }

            return View(tCompany);
        }
        public async Task<IActionResult> ListDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tCompany = await _context.TCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tCompany == null)
            {
                return NotFound();
            }

            return View(tCompany);
        }

        // GET: TCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanieName,Siret,Description,CreationDate,Phone,Website")] TCompany tCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tCompany);
        }

        // GET: TCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tCompany = await _context.TCompanies.FindAsync(id);
            if (tCompany == null)
            {
                return NotFound();
            }
            return View(tCompany);
        }

        // POST: TCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanieName,Siret,Description,CreationDate,Phone,Website")] TCompany tCompany)
        {
            if (id != tCompany.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TCompanyExists(tCompany.Id))
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
            return View(tCompany);
        }

        // GET: TCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tCompany = await _context.TCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tCompany == null)
            {
                return NotFound();
            }

            return View(tCompany);
        }

        // POST: TCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tCompany = await _context.TCompanies.FindAsync(id);
            if (tCompany != null)
            {
                _context.TCompanies.Remove(tCompany);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TCompanyExists(int id)
        {
            return _context.TCompanies.Any(e => e.Id == id);
        }
    }
}
