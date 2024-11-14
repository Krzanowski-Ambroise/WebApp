using Microsoft.AspNetCore.Identity;
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
    public class MyCompaniesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WebAppContext _context;

        public MyCompaniesModel(UserManager<ApplicationUser> userManager, WebAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<TCompany> AssociatedCompanies { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                AssociatedCompanies = new List<TCompany>();
                return;
            }

            AssociatedCompanies = await _context.UserCompanieAssociations
                .Where(uca => uca.UserId == user.Id)
                .Select(uca => uca.Company)
                .ToListAsync();
        }
    }
}
