using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class TPoste
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? ResponsabilityRank { get; set; }

    public virtual ICollection<TExerce> TExerces { get; set; } = new List<TExerce>();

    public virtual ICollection<TJobPost> TJobPosts { get; set; } = new List<TJobPost>();
}
