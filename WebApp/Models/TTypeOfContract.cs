using System;
using System.Collections.Generic;

namespace WebApp.Models;

public partial class TTypeOfContract
{
    public int Id { get; set; }

    public string ContractName { get; set; } = null!;

    public virtual ICollection<TExerce> TExerces { get; set; } = new List<TExerce>();

    public virtual ICollection<TJobPost> TJobPosts { get; set; } = new List<TJobPost>();
}
