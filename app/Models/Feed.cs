using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_real_social_app.Models;
public class Feed
{
    public string? Text { get; set; }
    public string? PostPhoto { get; set; }
    public string? UserPhoto { get; set; }
    public DateTime Timestamp { get; set; }

}