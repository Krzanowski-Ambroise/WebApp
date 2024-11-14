using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class TRequirementType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<TJobPostRequirement> TJobPostRequirements { get; set; } = new List<TJobPostRequirement>();

    public virtual ICollection<TUserRequirement> TUserRequirements { get; set; } = new List<TUserRequirement>();
}
