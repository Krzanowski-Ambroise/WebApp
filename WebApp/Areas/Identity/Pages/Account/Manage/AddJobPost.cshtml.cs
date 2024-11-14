using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Enterprise")]
    public class AddJobPostModel : PageModel
    {
        private readonly WebAppContext _context;

        public AddJobPostModel(WebAppContext context)
        {
            _context = context;
        }

        public string CompanyName { get; set; }

        [BindProperty]
        public TJobPost JobPost { get; set; }

        public List<TTypeOfContract> TypeOfContracts { get; set; }
        public List<TSite> Sites { get; set; }
        public List<TPoste> Postes { get; set; }  // Ajout de la liste Postes

        [BindProperty(SupportsGet = true)]
        public int CompanyId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TypeOfContracts = await _context.TTypeOfContracts.ToListAsync();
            Sites = await _context.TSites.Where(s => s.CompanieId == CompanyId).ToListAsync();
            Postes = await _context.TPostes.ToListAsync();  // Récupération des postes

            var company = await _context.TCompanies.FindAsync(CompanyId);
            if (company == null)
            {
                return NotFound();
            }

            CompanyName = company.CompanieName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TypeOfContracts = await _context.TTypeOfContracts.ToListAsync();
                Sites = await _context.TSites.Where(s => s.CompanieId == CompanyId).ToListAsync();
                Postes = await _context.TPostes.ToListAsync();  // Recharger les postes en cas d'échec de validation
                return Page();
            }

            JobPost.CompanieId = CompanyId;
            _context.TJobPosts.Add(JobPost);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ViewPosts", new { companyId = CompanyId });
        }
    }
}
