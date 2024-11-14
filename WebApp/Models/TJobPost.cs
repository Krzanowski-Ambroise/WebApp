using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public partial class TJobPost
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int? PosteId { get; set; }

    public string? Description { get; set; }

    public int? TypeOfContractId { get; set; }

    public int? SiteId { get; set; }

    public int? CompanieId { get; set; }

    public virtual TCompany? Companie { get; set; }

    public virtual TPoste? Poste { get; set; }

    public virtual TSite? Site { get; set; }
    [NotMapped]
    public double CompatibilityScore { get; set; }

    public virtual ICollection<TJobPostRequirement> TJobPostRequirements { get; set; } = new List<TJobPostRequirement>();
    public virtual TTypeOfContract? TypeOfContract { get; set; }
}
