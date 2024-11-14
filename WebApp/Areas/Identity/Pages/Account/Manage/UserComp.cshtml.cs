using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Areas.Identity.Data;
using WebApp.Models;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Membre")]
    public class UserCompModel : PageModel
    {
        private readonly WebAppContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCompModel(WebAppContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<TRequirementType> RequirementTypes { get; set; }
        public List<TUserRequirement> UserRequirements { get; set; }

        public async Task OnGetAsync()
        {
            RequirementTypes = await _context.TRequirementTypes.ToListAsync();
            ViewData["RequirementTypes"] = RequirementTypes;

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                UserRequirements = await _context.TUserRequirements
                    .Include(ur => ur.RequirementType)
                    .Where(ur => ur.UserId == user.Id)
                    .ToListAsync();
            }
            else
            {
                UserRequirements = new List<TUserRequirement>();
            }
        }


    }
}
