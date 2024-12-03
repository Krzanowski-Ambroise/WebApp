using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Identity.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WebAppContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, WebAppContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListUser()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<(ApplicationUser User, IList<string> Roles)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add((user, roles));
            }

            return View(userRoles);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = (User: user, Roles: roles);
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRequirements = _context.TUserRequirements.Where(ur => ur.UserId == user.Id);
            _context.TUserRequirements.RemoveRange(userRequirements);

            var userExerces = _context.TExerces.Where(ex => ex.UserId == user.Id);
            _context.TExerces.RemoveRange(userExerces);

            var userCompanyAssociations = _context.UserCompanieAssociations.Where(uc => uc.UserId == user.Id);
            _context.UserCompanieAssociations.RemoveRange(userCompanyAssociations);

            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return View("Error");
            }

            return RedirectToAction("ListUser");
        }

        // Affiche la gestion des entreprises associées à l'utilisateur
        public async Task<IActionResult> ManageCompanies(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .Include(u => u.UserCompanieAssociations)
                .ThenInclude(uca => uca.Company)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = userId;
            ViewData["UserName"] = user.UserName;
            ViewData["AssociatedCompanies"] = user.UserCompanieAssociations.Select(uca => uca.Company).ToList();
            ViewData["AvailableCompanies"] = new SelectList(_context.TCompanies, "Id", "CompanieName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompany(UserCompanyAssociationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Vérifie si l'association existe déjà
                bool associationExists = _context.UserCompanieAssociations
                    .Any(uca => uca.UserId == model.UserId && uca.CompanyId == model.CompanyId);

                if (associationExists)
                {
                    // Ajoute un message d'erreur si l'association existe déjà
                    ModelState.AddModelError(string.Empty, "Cette entreprise est déjà attribuée à cet utilisateur.");

                    // Recharge les données pour la vue `ManageCompanies`
                    ViewData["AvailableCompanies"] = new SelectList(_context.TCompanies, "Id", "CompanieName");
                    ViewData["UserId"] = model.UserId;
                    ViewData["UserName"] = (await _userManager.FindByIdAsync(model.UserId))?.UserName;
                    ViewData["AssociatedCompanies"] = _context.UserCompanieAssociations
                        .Where(uca => uca.UserId == model.UserId)
                        .Select(uca => uca.Company)
                        .ToList();

                    // Reste sur la vue `ManageCompanies` pour afficher le message d'erreur
                    return View("ManageCompanies");
                }

                // Crée l'association si elle n'existe pas encore
                var association = new UserCompanieAssociation
                {
                    UserId = model.UserId,
                    CompanyId = model.CompanyId
                };

                _context.UserCompanieAssociations.Add(association);
                await _context.SaveChangesAsync();

                return RedirectToAction("ManageCompanies", new { userId = model.UserId });
            }

            // Recharge les données pour la vue `ManageCompanies` en cas d'erreur de validation
            ViewData["AvailableCompanies"] = new SelectList(_context.TCompanies, "Id", "CompanieName");
            ViewData["UserId"] = model.UserId;
            ViewData["UserName"] = (await _userManager.FindByIdAsync(model.UserId))?.UserName;
            ViewData["AssociatedCompanies"] = _context.UserCompanieAssociations
                .Where(uca => uca.UserId == model.UserId)
                .Select(uca => uca.Company)
                .ToList();

            return View("ManageCompanies");
        }


        // Supprime une entreprise associée à l'utilisateur
        [HttpPost]
        public async Task<IActionResult> RemoveCompany(string userId, int companyId)
        {
            var association = await _context.UserCompanieAssociations
                .FirstOrDefaultAsync(uca => uca.UserId == userId && uca.CompanyId == companyId);

            if (association != null)
            {
                _context.UserCompanieAssociations.Remove(association);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageCompanies", new { userId });
        }
        // Affiche la vue pour ajouter un rôle
        public async Task<IActionResult> AddRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("L'ID utilisateur est requis.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Utilisateur introuvable.");
            }

            // Récupération des rôles
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var availableRoles = allRoles.Except(userRoles).ToList();

            // Création du modèle
            var model = new AddRoleViewModel
            {
                UserId = id,
                UserName = user.UserName,
                AvailableRoles = availableRoles,
                UserRoles = userRoles.ToList()
            };

            // Retourne une vue complète
            return View(model); // Assurez-vous que la vue AddRole.cshtml existe
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.SelectedRole))
            {
                ModelState.AddModelError(string.Empty, "Les informations nécessaires ne sont pas complètes.");
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("Utilisateur introuvable.");
            }

            if (await _userManager.IsInRoleAsync(user, model.SelectedRole))
            {
                ModelState.AddModelError(string.Empty, "L'utilisateur a déjà ce rôle.");
                return View(model);
            }

            var result = await _userManager.AddToRoleAsync(user, model.SelectedRole);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("ListUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Les informations nécessaires ne sont pas complètes.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Utilisateur introuvable.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Erreur lors de la suppression du rôle.";
            }

            return RedirectToAction("AddRole", new { id = userId }); // Recharge la vue
        }



    }
}
