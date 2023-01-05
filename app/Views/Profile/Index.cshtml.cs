using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using D_real_social_app.Data;
using D_real_social_app.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace D_real_social_app.Controllers;

[Authorize]
public class Profile : Controller
{

    private readonly SocialAppContext _context;

    public Profile(SocialAppContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        return View();
    }
}