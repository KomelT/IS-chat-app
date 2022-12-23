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

namespace D_real_social_app.Controllers;

public class FeedController : Controller
{

    private readonly SocialAppContext _context;

    public FeedController(SocialAppContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ViewBag.User = userId;

        var sql = "SELECT id, [Post].Text AS Text, [Post].Photo AS PostPhoto, [Post].Timestamp as Timestamp, [User].Photo, [User].N AS UserPhoto FROM [Post] INNER JOIN [User] ON ([Post].UserID = [User].id) WHERE [Post].UserID = ANY ( SELECT UserID FROM Connection WHERE UserID = '" + userId + "' OR UserID2 = '" + userId + "' UNION SELECT UserID2 FROM Connection WHERE UserID = '" + userId + "' OR UserID2 = '" + userId + "') ORDER BY [Post].Timestamp DESC";
        var posts = await _context.Feed.FromSqlRaw(sql).ToListAsync();

        return View(posts);
    }
}