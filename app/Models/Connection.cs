using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_real_social_app.Models;
public class Connection
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int UserID { get; }
    public int UserID2 { get; }
}