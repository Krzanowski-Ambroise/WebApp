using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public partial class TJobPostRequirement
{
    public int Id { get; set; }

    public int? JobPostId { get; set; }

    public int? RequirementTypeId { get; set; }

    [Range(0, 10, ErrorMessage = "L'évaluation personnelle doit être entre 0 et 10.")]
    public int Evaluation { get; set; }

    public int? YearsOfExperience { get; set; }

    public virtual TJobPost? JobPost { get; set; }

    public virtual TRequirementType? RequirementType { get; set; }
}
