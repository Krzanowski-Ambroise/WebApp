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
    public class TJobPostRequirementsController : Controller
    {
        private readonly WebAppContext _context;

        public TJobPostRequirementsController(WebAppContext context)
        {
            _context = context;
        }

        // GET: TJobPostRequirements
        public async Task<IActionResult> Index()
        {
            var webAppContext = _context.TJobPostRequirements.Include(t => t.JobPost).Include(t => t.RequirementType);
            return View(await webAppContext.ToListAsync());
        }

        // GET: TJobPostRequirements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tJobPostRequirement = await _context.TJobPostRequirements
                .Include(t => t.JobPost)
                .Include(t => t.RequirementType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tJobPostRequirement == null)
            {
                return NotFound();
            }

            return View(tJobPostRequirement);
        }

        // GET: TJobPostRequirements/Create
        public IActionResult Create()
        {
            ViewData["JobPostId"] = new SelectList(_context.TJobPosts, "Id", "Id");
            ViewData["RequirementTypeId"] = new SelectList(_context.TRequirementTypes, "Id", "Id");
            return View();
        }

        // POST: TJobPostRequirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JobPostId,RequirementTypeId,Evaluation,YearsOfExperience")] TJobPostRequirement tJobPostRequirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tJobPostRequirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobPostId"] = new SelectList(_context.TJobPosts, "Id", "Id", tJobPostRequirement.JobPostId);
            ViewData["RequirementTypeId"] = new SelectList(_context.TRequirementTypes, "Id", "Id", tJobPostRequirement.RequirementTypeId);
            return View(tJobPostRequirement);
        }

        // GET: TJobPostRequirements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tJobPostRequirement = await _context.TJobPostRequirements.FindAsync(id);
            if (tJobPostRequirement == null)
            {
                return NotFound();
            }
            ViewData["JobPostId"] = new SelectList(_context.TJobPosts, "Id", "Id", tJobPostRequirement.JobPostId);
            ViewData["RequirementTypeId"] = new SelectList(_context.TRequirementTypes, "Id", "Id", tJobPostRequirement.RequirementTypeId);
            return View(tJobPostRequirement);
        }

        // POST: TJobPostRequirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobPostId,RequirementTypeId,Evaluation,YearsOfExperience")] TJobPostRequirement tJobPostRequirement)
        {
            if (id != tJobPostRequirement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tJobPostRequirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TJobPostRequirementExists(tJobPostRequirement.Id))
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
            ViewData["JobPostId"] = new SelectList(_context.TJobPosts, "Id", "Id", tJobPostRequirement.JobPostId);
            ViewData["RequirementTypeId"] = new SelectList(_context.TRequirementTypes, "Id", "Id", tJobPostRequirement.RequirementTypeId);
            return View(tJobPostRequirement);
        }

        // GET: TJobPostRequirements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tJobPostRequirement = await _context.TJobPostRequirements
                .Include(t => t.JobPost)
                .Include(t => t.RequirementType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tJobPostRequirement == null)
            {
                return NotFound();
            }

            return View(tJobPostRequirement);
        }

        // POST: TJobPostRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tJobPostRequirement = await _context.TJobPostRequirements.FindAsync(id);
            if (tJobPostRequirement != null)
            {
                _context.TJobPostRequirements.Remove(tJobPostRequirement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TJobPostRequirementExists(int id)
        {
            return _context.TJobPostRequirements.Any(e => e.Id == id);
        }
    }
}
