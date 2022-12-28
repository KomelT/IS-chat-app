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

        //ViewBag.User = userId;

        var sql = "SELECT [Post].Text AS Text, [Post].Photo AS PostPhoto, [Post].Timestamp as Timestamp, [User].Photo AS UserPhoto, CONCAT([User].FirstName, ' ', [User].LastName) AS UserName FROM [Post] INNER JOIN [User] ON ([Post].UserID = [User].id) WHERE [Post].UserID = '" + userId + "' ORDER BY [Post].Timestamp DESC";
        var posts = await _context.Feed.FromSqlRaw(sql).ToListAsync();

        return View(posts);
    }

    // 

    public async Task<IActionResult> uploadFile([FromForm] IFormFile file, String text)
    {

        if (text == null)
            return BadRequest("Post Text is missing!!");

        var fileName = "";
        if (file != null)
        {
            fileName = "/img/" + file.FileName;
            using (Stream fileStream = new FileStream("/app/wwwroot" + fileName, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        //var userId = "sdsadsadsadas";

        var sql = "INSERT INTO [Post] (UserID, Text, Photo, Timestamp) VALUES ('" + userId + "', '" + text + "', '" + fileName + "', '" + DateTime.Now + "')";
        _context.Database.ExecuteSqlRaw(sql);

        return Redirect("/feed");

    }


}