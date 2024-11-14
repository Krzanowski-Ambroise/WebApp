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
    public class TExercesController : Controller
    {
        private readonly WebAppContext _context;

        public TExercesController(WebAppContext context)
        {
            _context = context;
        }

        // GET: TExerces
        public async Task<IActionResult> Index()
        {
            var webAppContext = _context.TExerces.Include(t => t.Poste).Include(t => t.Site).Include(t => t.TypeOfContract).Include(t => t.User);

            return View(await webAppContext.ToListAsync());
        }

        // GET: TExerces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tExerce = await _context.TExerces
                .Include(t => t.Poste)
                .Include(t => t.Site)
                .Include(t => t.TypeOfContract)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tExerce == null)
            {

                return NotFound();
            }

            return View(tExerce);
        }

        // GET: TExerces/Create
        public IActionResult Create()
        {
            ViewData["PosteId"] = new SelectList(_context.TPostes, "Id", "Title");
            ViewData["SiteId"] = new SelectList(_context.TSites, "Id", "Title");
            ViewData["TypeOfContractId"] = new SelectList(_context.TTypeOfContracts, "Id", "ContractName");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName");
            return View();
        }

        // POST: TExerces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SiteId,PosteId,UserId,TypeOfContractId,StartDate,EndDate")] TExerce tExerce)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tExerce);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PosteId"] = new SelectList(_context.TPostes, "Id", "Title", tExerce.PosteId);
            ViewData["SiteId"] = new SelectList(_context.TSites, "Id", "Title", tExerce.SiteId);
            ViewData["TypeOfContractId"] = new SelectList(_context.TTypeOfContracts, "Id", "ContractName", tExerce.TypeOfContractId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", tExerce.UserId);
            return View(tExerce);
        }

        // GET: TExerces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tExerce = await _context.TExerces.FindAsync(id);
            if (tExerce == null)
            {
                return NotFound();
            }
            ViewData["PosteId"] = new SelectList(_context.TPostes, "Id", "Title", tExerce.PosteId);
            ViewData["SiteId"] = new SelectList(_context.TSites, "Id", "Title", tExerce.SiteId);
            ViewData["TypeOfContractId"] = new SelectList(_context.TTypeOfContracts, "Id", "ContractName", tExerce.TypeOfContractId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", tExerce.UserId);
            return View(tExerce);
        }

        // POST: TExerces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SiteId,PosteId,UserId,TypeOfContractId,StartDate,EndDate")] TExerce tExerce)
        {
            if (id != tExerce.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tExerce);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TExerceExists(tExerce.Id))
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
            ViewData["PosteId"] = new SelectList(_context.TPostes, "Id", "Title", tExerce.PosteId);
            ViewData["SiteId"] = new SelectList(_context.TSites, "Id", "Title", tExerce.SiteId);
            ViewData["TypeOfContractId"] = new SelectList(_context.TTypeOfContracts, "Id", "ContractName", tExerce.TypeOfContractId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", tExerce.UserId);
            return View(tExerce);
        }

        // GET: TExerces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tExerce = await _context.TExerces
                .Include(t => t.Poste)
                .Include(t => t.Site)
                .Include(t => t.TypeOfContract)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tExerce == null)
            {
                return NotFound();
            }

            return View(tExerce);
        }

        // POST: TExerces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tExerce = await _context.TExerces.FindAsync(id);
            if (tExerce != null)
            {
                _context.TExerces.Remove(tExerce);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TExerceExists(int id)
        {
            return _context.TExerces.Any(e => e.Id == id);
        }
    }
}
