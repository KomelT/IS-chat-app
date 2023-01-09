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

    public async Task<IActionResult> Index(string id)
    {

        ViewData["userId"] = id;


        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ViewData["myId"] = userId;


        var sql = "SELECT [Post].UserId as UserId, [Post].PostID AS PostID, [Post].Text AS Text, [Post].Photo AS PostPhoto, [Post].Timestamp as Timestamp, [User].Photo AS UserPhoto, CONCAT([User].FirstName, ' ', [User].LastName) AS UserName FROM [Post] INNER JOIN [User] ON ([Post].UserID = [User].id) WHERE [Post].UserID = ANY ( SELECT UserID FROM Connection WHERE UserID = '" + userId + "' OR UserID2 = '" + userId + "' UNION SELECT UserID2 FROM Connection WHERE UserID = '" + userId + "' OR UserID2 = '" + userId + "' UNION SELECT '" + userId + "' FROM Connection UNION SELECT CONCAT('" + userId + "', '') AS UserId ) ORDER BY [Post].Timestamp DESC";
        var posts = await _context.Feed.FromSqlRaw(sql).ToListAsync();

        foreach (Feed post in posts)
        {
            sql = "SELECT Comment.CommentID AS CommentID, Comment.PostID as PostID, Comment.UserID as UserID, [User].Photo as UserPhoto, CONCAT([User].FirstName, ' ', [User].LastName) AS UserName, Comment.Text AS Text FROM Comment INNER JOIN [User] ON [User].Id = Comment.UserId WHERE PostID = '" + post.PostID + "' ";
            var sqlRes1 = await _context.Comment.FromSqlRaw(sql).ToListAsync();
            post.Comments = sqlRes1;
        }

        sql = "SELECT * FROM [USER] WHERE Id ='" + id + "'";
        var sqlRes = await _context.User.FromSqlRaw(sql).ToListAsync();

        sql = "SELECT * FROM Connection WHERE (UserId = '" + id + "' AND UserId2 = '" + userId + "') OR (UserId = '" + userId + "' AND UserId2 = '" + id + "')";
        var connection = await _context.Connection.FromSqlRaw(sql).ToListAsync();

        ViewData["isConnected"] = connection.Count;

        ViewData["userName"] = sqlRes[0].FirstName + " " + sqlRes[0].LastName;
        ViewData["userPhoto"] = sqlRes[0].Photo;

        return View(posts);
    }

    public async Task<IActionResult> toggleConnection([FromForm] String id)
    {

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        var sql = "SELECT * FROM Connection WHERE (UserId = '" + id + "' AND UserId2 = '" + userId + "') OR (UserId = '" + userId + "' AND UserId2 = '" + id + "')";
        var connection = await _context.Connection.FromSqlRaw(sql).ToListAsync();

        if (connection.Count == 0)
        {
            sql = "INSERT INTO Connection (UserId, UserId2) Values ('" + id + "', '" + userId + "')";
            _context.Database.ExecuteSqlRaw(sql);
        }
        else
        {
            sql = "DELETE FROM Connection WHERE (UserId = '" + id + "' AND UserId2 = '" + userId + "') OR (UserId = '" + userId + "' AND UserId2 = '" + id + "')";
            _context.Database.ExecuteSqlRaw(sql);
        }


        return Redirect("/profile/?id=" + id);
    }
}