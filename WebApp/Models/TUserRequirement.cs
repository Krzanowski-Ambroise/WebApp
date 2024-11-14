using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models;

public partial class TUserRequirement
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int RequirementTypeId { get; set; }

    [Required]
    [Range(0, 10, ErrorMessage = "L'évaluation personnelle doit être entre 0 et 10.")]
    public int PersonnalEvaluation { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    public virtual TRequirementType? RequirementType { get; set; }

    public virtual ApplicationUser? User { get; set; } = null!;
}
