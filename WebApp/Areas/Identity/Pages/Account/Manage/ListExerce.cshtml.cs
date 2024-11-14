using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Membre")]
    public class ListExerceModel : PageModel
    {
        private readonly WebAppContext _context;

        public ListExerceModel(WebAppContext context)
        {
            _context = context;
        }

        public List<TExerce> Exerces { get; set; } = new List<TExerce>();
        public List<TSite> Sites { get; set; } = new List<TSite>();
        public List<TPoste> Postes { get; set; } = new List<TPoste>();
        public List<TTypeOfContract> TypeOfContracts { get; set; } = new List<TTypeOfContract>();

        [BindProperty]
        public TExerce NewExerce { get; set; } = new TExerce();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound("Utilisateur non trouvé.");
            }

            Exerces = await _context.TExerces
                .Include(e => e.Site)
                .Include(e => e.Poste)
                .Include(e => e.TypeOfContract)
                .Where(e => e.UserId == userId)
                .ToListAsync();

            // Inclure les informations de l'entreprise associée à chaque site
            Sites = await _context.TSites
                .Include(s => s.Companie)
                .ToListAsync();

            Postes = await _context.TPostes.ToListAsync();
            TypeOfContracts = await _context.TTypeOfContracts.ToListAsync();

            return Page();
        }


        public async Task<IActionResult> OnPostAddExerceAsync()
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync();
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound("Utilisateur non trouvé.");
            }

            NewExerce.UserId = userId; // Associe l'exercice à l'utilisateur connecté
            _context.TExerces.Add(NewExerce);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveExerceAsync(int id)
        {
            var exerce = await _context.TExerces.FindAsync(id);
            if (exerce != null)
            {
                _context.TExerces.Remove(exerce);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
