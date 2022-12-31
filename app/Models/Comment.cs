using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_real_social_app.Models;
public class Comment
{
    public int CommentID { get; set; }
    public string? UserID { get; set; }
    public int PostID { get; set; }
    public string? Text { get; set; }
}