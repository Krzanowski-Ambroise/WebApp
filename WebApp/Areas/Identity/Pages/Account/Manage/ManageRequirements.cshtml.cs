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
    [Authorize(Roles = "Membre")]
    public class ManageRequirementsModel : PageModel
    {
        private readonly WebAppContext _context;

        public ManageRequirementsModel(WebAppContext context)
        {
            _context = context;
        }

        public int CompanyId { get; set; }
        public string JobPostTitle { get; set; }
        public List<TJobPostRequirement> Requirements { get; set; }
        public List<TRequirementType> RequirementTypes { get; set; }

        [BindProperty]
        public TJobPostRequirement NewRequirement { get; set; }

        [BindProperty(SupportsGet = true)]
        public int JobPostId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var jobPost = await _context.TJobPosts
                .Include(j => j.Companie)
                .FirstOrDefaultAsync(j => j.Id == JobPostId);

            if (jobPost == null)
            {
                return NotFound();
            }

            CompanyId = jobPost.CompanieId ?? 0;
            JobPostTitle = jobPost.Title;
            await LoadDataAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddRequirementAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            // Vérifier si le type d'exigence est déjà associé à ce JobPost
            bool requirementExists = await _context.TJobPostRequirements
                .AnyAsync(r => r.JobPostId == JobPostId && r.RequirementTypeId == NewRequirement.RequirementTypeId);

            if (requirementExists)
            {
                // Ajouter un message d'erreur
                ModelState.AddModelError(string.Empty, "Ce type d'exigence est déjà présent pour ce poste.");
                await LoadDataAsync();
                return Page();
            }

            // Ajouter la nouvelle exigence
            NewRequirement.JobPostId = JobPostId;
            _context.TJobPostRequirements.Add(NewRequirement);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { jobPostId = JobPostId });
        }

        public async Task<IActionResult> OnPostRemoveRequirementAsync(int requirementId)
        {
            var requirement = await _context.TJobPostRequirements.FindAsync(requirementId);
            if (requirement != null)
            {
                _context.TJobPostRequirements.Remove(requirement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { jobPostId = JobPostId });
        }

        private async Task LoadDataAsync()
        {
            // Charger les données nécessaires pour la page
            Requirements = await _context.TJobPostRequirements
                .Where(r => r.JobPostId == JobPostId)
                .Include(r => r.RequirementType)
                .ToListAsync();

            RequirementTypes = await _context.TRequirementTypes.ToListAsync();
        }
    }
}
