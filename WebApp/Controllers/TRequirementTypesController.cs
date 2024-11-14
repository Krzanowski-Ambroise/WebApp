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
    public class TRequirementTypesController : Controller
    {
        private readonly WebAppContext _context;

        public TRequirementTypesController(WebAppContext context)
        {
            _context = context;
        }

        // GET: TRequirementTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TRequirementTypes.ToListAsync());
        }

        // GET: TRequirementTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tRequirementType = await _context.TRequirementTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tRequirementType == null)
            {
                return NotFound();
            }

            return View(tRequirementType);
        }

        // GET: TRequirementTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TRequirementTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeName")] TRequirementType tRequirementType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tRequirementType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tRequirementType);
        }

        // GET: TRequirementTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tRequirementType = await _context.TRequirementTypes.FindAsync(id);
            if (tRequirementType == null)
            {
                return NotFound();
            }
            return View(tRequirementType);
        }

        // POST: TRequirementTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeName")] TRequirementType tRequirementType)
        {
            if (id != tRequirementType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tRequirementType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TRequirementTypeExists(tRequirementType.Id))
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
            return View(tRequirementType);
        }

        // GET: TRequirementTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tRequirementType = await _context.TRequirementTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tRequirementType == null)
            {
                return NotFound();
            }

            return View(tRequirementType);
        }

        // POST: TRequirementTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tRequirementType = await _context.TRequirementTypes.FindAsync(id);
            if (tRequirementType != null)
            {
                _context.TRequirementTypes.Remove(tRequirementType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TRequirementTypeExists(int id)
        {
            return _context.TRequirementTypes.Any(e => e.Id == id);
        }
    }
}
