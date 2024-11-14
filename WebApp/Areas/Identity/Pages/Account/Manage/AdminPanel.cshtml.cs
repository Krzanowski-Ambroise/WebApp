using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
