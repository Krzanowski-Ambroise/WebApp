using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public partial class TSite
{
    public int Id { get; set; }

    public int? CompanieId { get; set; }

    public string Title { get; set; } = null!;

    public string? Street { get; set; }

    public string? City { get; set; }

    public int? Postcode { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    [NotMapped]
    public double? DistanceFromUser { get; set; }


    public virtual TCompany? Companie { get; set; }

    public virtual ICollection<TExerce> TExerces { get; set; } = new List<TExerce>();

    public virtual ICollection<TJobPost> TJobPosts { get; set; } = new List<TJobPost>();
}
