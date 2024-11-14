using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Identity.Data;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Membre")]
    public class ViewPostsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WebAppContext _context;

        public ViewPostsModel(UserManager<ApplicationUser> userManager, WebAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<TJobPost> JobPosts { get; set; }
        public string CompanyName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CompanyId { get; set; }

        public async Task<IActionResult> OnGetAsync(int companyId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Utilisateur non trouvé.");
            }

            // Vérifier que l'utilisateur est bien associé à cette entreprise
            var isAssociated = await _context.UserCompanieAssociations
                .AnyAsync(uca => uca.UserId == user.Id && uca.CompanyId == companyId);

            if (!isAssociated)
            {
                return Forbid();
            }

            // Récupérer les informations de l'entreprise
            var company = await _context.TCompanies.FindAsync(companyId);
            if (company == null)
            {
                return NotFound("Entreprise non trouvée.");
            }

            CompanyName = company.CompanieName;

            // Récupérer les posts associés à l'entreprise, incluant les exigences
            JobPosts = await _context.TJobPosts
                .Where(jp => jp.CompanieId == companyId)
                .Include(jp => jp.TypeOfContract)
                .Include(jp => jp.Poste)
                .Include(jp => jp.Site)
                .Include(jp => jp.TJobPostRequirements) // Inclure les exigences
                    .ThenInclude(r => r.RequirementType) // Inclure le type d'exigence si nécessaire
                .ToListAsync();

            return Page();
        }

        [Authorize(Roles = "Enterprise")]
        public async Task<IActionResult> OnPostDeleteJobPostAsync(int jobPostId)
        {
            var jobPost = await _context.TJobPosts.FindAsync(jobPostId);
            if (jobPost != null && jobPost.CompanieId == CompanyId)
            {
                _context.TJobPosts.Remove(jobPost);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { companyId = CompanyId });
        }
    }
}
