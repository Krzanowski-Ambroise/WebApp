using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TUserRequirementsController : Controller
    {
        private readonly WebAppContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TUserRequirementsController(WebAppContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult AddRequirement()
        {
            // Charge les types de compétence et les passe à la vue via ViewBag
            ViewBag.RequirementTypes = _context.TRequirementTypes.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Membre")]
        public async Task<IActionResult> AddRequirement(TUserRequirement tUserRequirement)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            tUserRequirement.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(tUserRequirement);
                await _context.SaveChangesAsync();
                return Redirect("/Identity/Account/Manage/UserComp"); // Retourne à UserComp après l'ajout
            }
            ViewBag.RequirementTypes = _context.TRequirementTypes.ToList();
            return View(tUserRequirement); // Affiche la vue avec le formulaire en cas d'erreur de validation
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Membre")]
        public async Task<IActionResult> DeleteRequirement(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var requirement = await _context.TUserRequirements
                .Where(r => r.Id == id && r.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (requirement == null)
            {
                return NotFound();
            }

            _context.TUserRequirements.Remove(requirement);
            await _context.SaveChangesAsync();

            return Redirect("/Identity/Account/Manage/UserComp"); // Redirige vers la page des compétences
        }


        // GET: TUserRequirements
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var webAppContext = _context.TUserRequirements.Include(t => t.RequirementType).Include(t => t.User);
            return View(await webAppContext.ToListAsync());
        }

        // GET: TUserRequirements/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUserRequirement = await _context.TUserRequirements
                .Include(t => t.RequirementType)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tUserRequirement == null)
            {
                return NotFound();
            }

            return View(tUserRequirement);
        }

        // GET: TUserRequirements/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RequirementTypeId"] = new SelectList(_context.TRequirementTypes, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: TUserRequirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,UserId,RequirementTypeId,PersonnalEvaluation,StartDate,EndDate")] TUserRequirement tUserRequirement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tUserRequirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequirementTypeId"] = new SelectList(_context.TRequirementTypes, "Id", "Id", tUserRequirement.RequirementTypeId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", tUserRequirement.UserId);
            return View(tUserRequirement);
        }

        // GET: TUserRequirements/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUserRequirement = await _context.TUserRequirements.FindAsync(id);
            if (tUserRequirement == null)
            {
                return NotFound();
            }
            ViewData["RequirementTypeId"] = new SelectList(_context.TRequirementTypes, "Id", "Id", tUserRequirement.RequirementTypeId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", tUserRequirement.UserId);
            return View(tUserRequirement);
        }

        // POST: TUserRequirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,RequirementTypeId,PersonnalEvaluation,StartDate,EndDate")] TUserRequirement tUserRequirement)
        {
            if (id != tUserRequirement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tUserRequirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TUserRequirementExists(tUserRequirement.Id))
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
            ViewData["RequirementTypeId"] = new SelectList(_context.TRequirementTypes, "Id", "Id", tUserRequirement.RequirementTypeId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", tUserRequirement.UserId);
            return View(tUserRequirement);
        }

        // GET: TUserRequirements/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUserRequirement = await _context.TUserRequirements
                .Include(t => t.RequirementType)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tUserRequirement == null)
            {
                return NotFound();
            }

            return View(tUserRequirement);
        }

        // POST: TUserRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tUserRequirement = await _context.TUserRequirements.FindAsync(id);
            if (tUserRequirement != null)
            {
                _context.TUserRequirements.Remove(tUserRequirement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TUserRequirementExists(int id)
        {
            return _context.TUserRequirements.Any(e => e.Id == id);
        }



    }
}
