using System;
using System.Collections.Generic;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models;

public partial class TExerce
{
    public int Id { get; set; }

    public int? SiteId { get; set; }

    public int? PosteId { get; set; }

    public string? UserId { get; set; }

    public int? TypeOfContractId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual TPoste? Poste { get; set; }

    public virtual TSite? Site { get; set; }

    public virtual TTypeOfContract? TypeOfContract { get; set; }

    public virtual ApplicationUser? User { get; set; }

}
