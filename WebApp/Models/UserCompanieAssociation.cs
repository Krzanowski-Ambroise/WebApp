using System;
using System.Collections.Generic;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models;

public partial class UserCompanieAssociation
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int CompanyId { get; set; }

    public virtual TCompany Company { get; set; } = null!;

    public virtual ApplicationUser? User { get; set; } = null!;
}
