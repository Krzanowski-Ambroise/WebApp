using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Identity.Data;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Enterprise")]
    public class ManageCompanyModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WebAppContext _context;

        private async Task<(double? Latitude, double? Longitude)> GetCoordinatesAsync(string address)
        {
            // Appel à l'API pour récupérer les coordonnées
            using (var client = new HttpClient())
            {
                string apiKey = "97063609c27c4515916e933ee4957fab";
                string url = $"https://api.geoapify.com/v1/geocode/search?text={Uri.EscapeDataString(address)}&apiKey={apiKey}";

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonDocument.Parse(json);

                    var lat = data.RootElement.GetProperty("features")[0]
                        .GetProperty("geometry").GetProperty("coordinates")[1].GetDouble();
                    var lon = data.RootElement.GetProperty("features")[0]
                        .GetProperty("geometry").GetProperty("coordinates")[0].GetDouble();

                    return (lat, lon);
                }
            }
            return (null, null);
        }


        public ManageCompanyModel(UserManager<ApplicationUser> userManager, WebAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public TCompany Company { get; set; }
        public List<TSite> Sites { get; set; }
        public List<ApplicationUser> CompanyMembers { get; set; }
        public List<ApplicationUser> AvailableMembers { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CompanyId { get; set; }
        public Dictionary<string, string> MemberRoles { get; set; } = new Dictionary<string, string>();

        public async Task<IActionResult> OnGetAsync(int companyId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var isAssociated = await _context.UserCompanieAssociations
                .AnyAsync(uca => uca.UserId == user.Id && uca.CompanyId == companyId);

            if (!isAssociated)
            {
                return Forbid();
            }

            Company = await _context.TCompanies.FindAsync(companyId);
            if (Company == null)
            {
                return NotFound("Company not found.");
            }

            Sites = await _context.TSites.Where(s => s.CompanieId == companyId).ToListAsync();

            // Load company members
            CompanyMembers = await _context.UserCompanieAssociations
                .Where(uca => uca.CompanyId == companyId)
                .Select(uca => uca.User)
                .ToListAsync();

            // Load roles for each member
            foreach (var member in CompanyMembers)
            {
                var roles = await _userManager.GetRolesAsync(member);
                MemberRoles[member.Id] = string.Join(", ", roles);
            }

            AvailableMembers = await _userManager.Users
                .Where(u => !CompanyMembers.Select(cm => cm.Id).Contains(u.Id))
                .ToListAsync();

            CompanyId = companyId;
            return Page();
        }

        public async Task<IActionResult> OnPostAddMemberAsync(string userId)
        {
            if (CompanyId <= 0 || string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(new { companyId = CompanyId });
            }

            var association = new UserCompanieAssociation
            {
                UserId = userId,
                CompanyId = CompanyId
            };

            _context.UserCompanieAssociations.Add(association);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { companyId = CompanyId });
        }



        public async Task<IActionResult> OnPostRemoveMemberAsync(string userId)
        {
            var association = await _context.UserCompanieAssociations
                .FirstOrDefaultAsync(uca => uca.UserId == userId && uca.CompanyId == CompanyId);
            if (association != null)
            {
                _context.UserCompanieAssociations.Remove(association);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { companyId = CompanyId });
        }

        public async Task<IActionResult> OnPostAddSiteAsync(string title, string street, string city, int postcode)
        {
            if (!ModelState.IsValid || CompanyId <= 0)
            {
                return RedirectToPage(new { companyId = CompanyId });
            }

            // Construire l'adresse et récupérer les coordonnées
            var address = $"{street} {city} {postcode}";
            var (latitude, longitude) = await GetCoordinatesAsync(address);

            var site = new TSite
            {
                CompanieId = CompanyId,
                Title = title,
                Street = street,
                City = city,
                Postcode = postcode,
                Latitude = latitude,
                Longitude = longitude
            };

            _context.TSites.Add(site);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { companyId = CompanyId });
        }
        // Quand l'edit existera tout est pret !
        public async Task<IActionResult> OnPostEditSiteAsync(int siteId, string title, string street, string city, int postcode)
        {
            var site = await _context.TSites.FindAsync(siteId);
            if (site == null || site.CompanieId != CompanyId)
            {
                return RedirectToPage(new { companyId = CompanyId });
            }

            site.Title = title;
            site.Street = street;
            site.City = city;
            site.Postcode = postcode;

            // Vérifier si l'adresse est complète
            if (!string.IsNullOrEmpty(street) && !string.IsNullOrEmpty(city) && postcode != 0)
            {
                // Récupérer les nouvelles coordonnées
                var address = $"{street} {city} {postcode}";
                var (latitude, longitude) = await GetCoordinatesAsync(address);

                site.Latitude = latitude;
                site.Longitude = longitude;
            }
            else
            {
                // Supprimer les coordonnées si l'adresse est incomplète
                site.Latitude = null;
                site.Longitude = null;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage(new { companyId = CompanyId });
        }

        public async Task<IActionResult> OnPostRemoveSiteAsync(int siteId)
        {
            var site = await _context.TSites.FindAsync(siteId);
            if (site != null && site.CompanieId == CompanyId)
            {
                _context.TSites.Remove(site);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { companyId = CompanyId });
        }
    }
}
