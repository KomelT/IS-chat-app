using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_real_social_app.Models;

public class Connection
{
    public int ConnectionID { get; set; }
    public string? UserID { get; set; }
    public string? UserID2 { get; set; }
}