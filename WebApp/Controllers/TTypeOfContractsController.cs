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
    public class TTypeOfContractsController : Controller
    {
        private readonly WebAppContext _context;

        public TTypeOfContractsController(WebAppContext context)
        {
            _context = context;
        }

        // GET: TTypeOfContracts
        public async Task<IActionResult> Index()
        {
            return View(await _context.TTypeOfContracts.ToListAsync());
        }

        // GET: TTypeOfContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tTypeOfContract = await _context.TTypeOfContracts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tTypeOfContract == null)
            {
                return NotFound();
            }

            return View(tTypeOfContract);
        }

        // GET: TTypeOfContracts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TTypeOfContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContractName")] TTypeOfContract tTypeOfContract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tTypeOfContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tTypeOfContract);
        }

        // GET: TTypeOfContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tTypeOfContract = await _context.TTypeOfContracts.FindAsync(id);
            if (tTypeOfContract == null)
            {
                return NotFound();
            }
            return View(tTypeOfContract);
        }

        // POST: TTypeOfContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContractName")] TTypeOfContract tTypeOfContract)
        {
            if (id != tTypeOfContract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tTypeOfContract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TTypeOfContractExists(tTypeOfContract.Id))
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
            return View(tTypeOfContract);
        }

        // GET: TTypeOfContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tTypeOfContract = await _context.TTypeOfContracts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tTypeOfContract == null)
            {
                return NotFound();
            }

            return View(tTypeOfContract);
        }

        // POST: TTypeOfContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tTypeOfContract = await _context.TTypeOfContracts.FindAsync(id);
            if (tTypeOfContract != null)
            {
                _context.TTypeOfContracts.Remove(tTypeOfContract);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TTypeOfContractExists(int id)
        {
            return _context.TTypeOfContracts.Any(e => e.Id == id);
        }
    }
}
