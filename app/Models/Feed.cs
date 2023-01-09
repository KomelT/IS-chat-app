using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_real_social_app.Models;
public class Feed
{
    public string? UserName { get; set; }
    public string? UserId {get; set;}
    public string? Text { get; set; }
    public string? PostPhoto { get; set; }
    public int PostID { get; set; }
    public string? UserPhoto { get; set; }
    public DateTime Timestamp { get; set; }
    public List<Comment> Comments { get; set; }

}