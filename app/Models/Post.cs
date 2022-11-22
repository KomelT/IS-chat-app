using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_real_social_app.Models;
public class Post
{
    public int PostID { get; }
    public int UserID { get; set; }
    public string Text { get; set; }
}