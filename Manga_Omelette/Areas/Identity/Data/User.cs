using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MangaASP.Models;
using Microsoft.AspNetCore.Identity;

namespace Manga_Omelette.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string NameDisplay {  get; set; }

    public ICollection<Comment> Comments { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<FavoriteList> FavoriteLists { get; set; }
}

