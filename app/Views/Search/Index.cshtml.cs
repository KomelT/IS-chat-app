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
public class Search : Controller
{

    private readonly SocialAppContext _context;

    public Search(SocialAppContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        string text = HttpContext.Request.Query["text"];
        ViewData["Empty"] = true;

        if (!text.ToString().Equals(""))
        {
            var sql = "SELECT * FROM [User] WHERE UPPER(CONCAT(FirstName, ' ', LastName)) LIKE CONCAT('%', UPPER(REPLACE('" + text + "', ' ', '%')), '%')";
            var sqlRes = await _context.User.FromSqlRaw(sql).ToListAsync();

            if (sqlRes.Count != 0)
            {
                ViewData["Empty"] = false;
            }

            return View(sqlRes);
        }

        return View();

    }
}