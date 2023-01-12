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
using Microsoft.Data.SqlClient;

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

        var sql = "SELECT * FROM [User] WHERE Id = '" + userId + "'";
        var sqlRes = await _context.User.FromSqlRaw(sql).ToListAsync();

        ViewData["UserName"] = sqlRes[0].FirstName + " " + sqlRes[0].LastName;
        ViewData["UserPhoto"] = sqlRes[0].Photo;

        sql = "SELECT [Post].UserId as UserId, [Post].PostID AS PostID, [Post].Text AS Text, [Post].Photo AS PostPhoto, [Post].Timestamp as Timestamp, [User].Photo AS UserPhoto, CONCAT([User].FirstName, ' ', [User].LastName) AS UserName FROM [Post] INNER JOIN [User] ON ([Post].UserID = [User].id) WHERE [Post].UserID = ANY ( SELECT UserID FROM Connection WHERE UserID = '" + userId + "' OR UserID2 = '" + userId + "' UNION SELECT UserID2 FROM Connection WHERE UserID = '" + userId + "' OR UserID2 = '" + userId + "' UNION SELECT '" + userId + "' FROM Connection UNION SELECT CONCAT('" + userId + "', '') AS UserId ) ORDER BY [Post].Timestamp DESC";
        var posts = await _context.Feed.FromSqlRaw(sql).ToListAsync();

        foreach (Feed post in posts)
        {
            sql = "SELECT Comment.CommentID AS CommentID, Comment.PostID as PostID, Comment.UserID as UserID, [User].Photo as UserPhoto, CONCAT([User].FirstName, ' ', [User].LastName) AS UserName, Comment.Text AS Text FROM Comment INNER JOIN [User] ON [User].Id = Comment.UserId WHERE PostID = '" + post.PostID + "' ";
            var sqlRes1 = await _context.Comment.FromSqlRaw(sql).ToListAsync();
            post.Comments = sqlRes1;
        }

        return View(posts);
    }
    // 

    public async Task<IActionResult> uploadFile([FromForm] IFormFile file, String text)
    {

        if (text == null)
            return Redirect("/feed?pserr=Post%20text%20is%20missing%21");

        var fileName = "";
        if (file != null)
        {
            var end = file.FileName.Split(".")[file.FileName.Split(".").Length - 1];

            if (!(end == "jpg" || end == "png"))
            {
                return Redirect("/feed?pserr=Attachment%20must%20be%20in%20.jpg%20or%20.png%20format%21");
            }

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[32];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            fileName = "/img/uploads/" + finalString + "." + end;
            using (Stream fileStream = new FileStream("/app/wwwroot" + fileName, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        var sql = "INSERT INTO [Post] (UserID, Text, Photo, Timestamp) VALUES ('" + userId + "', '" + text + "', '" + fileName + "', '" + DateTime.Now + "')";
        _context.Database.ExecuteSqlRaw(sql);

        return Redirect("/feed");

    }

    public async Task<IActionResult> uploadComment([FromForm] String text, int postId)
    {
        if (text == null || postId == null)
        {
            return Redirect("/feed?pserr=Text%20is%20mandatory%20to%20post%20a%20comment%21");
        }

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        var sql = "INSERT INTO [Comment] (UserID, Text, PostID) VALUES ('" + userId + "', '" + text + "', '" + postId + "')";
        _context.Database.ExecuteSqlRaw(sql);
        return Redirect("/feed");
    }

}