using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace D_real_social_app.Models;
public class User : IdentityUser
{
    public string? Photo { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}