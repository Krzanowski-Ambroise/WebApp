using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApp.Models;

namespace WebApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; } = string.Empty;

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; } = string.Empty;

    [PersonalData]
    [Column(TypeName = "nvarchar(255)")]
    public string? Street { get; set; }  

    [PersonalData]
    [Column(TypeName = "nvarchar(255)")]
    public string? City { get; set; }

    [PersonalData]
    [Column(TypeName = "int")]
    public int? Postcode { get; set; }

    [PersonalData]
    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }


    public virtual ICollection<TUserRequirement> TUserRequirements { get; set; } = new List<TUserRequirement>(); // Initialisation
    public virtual ICollection<TExerce> TExerces { get; set; } = new List<TExerce>(); // Initialisation
    public virtual ICollection<UserCompanieAssociation> UserCompanieAssociations { get; set; } = new List<UserCompanieAssociation>(); // Initialisation
}


