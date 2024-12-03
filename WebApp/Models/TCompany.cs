using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public partial class TCompany
{
    public int Id { get; set; }

    public string CompanieName { get; set; } = null!;
    
    [MaxLength(11)]
    [StringLength(11, ErrorMessage = "Le champ Siret ne peut pas dépasser 11 caractères.")] 
    public string? Siret { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreationDate { get; set; }

    public int? Phone { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<TJobPost> TJobPosts { get; set; } = new List<TJobPost>();

    public virtual ICollection<TSite> TSites { get; set; } = new List<TSite>();

    public virtual ICollection<UserCompanieAssociation> UserCompanieAssociations { get; set; } = new List<UserCompanieAssociation>();
}
