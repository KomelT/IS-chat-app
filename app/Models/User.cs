using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_real_social_app.Models;
public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int UserID { get; set; }
    public string Photo { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Pass { get; set; }
    public string Usern { get; set; }
}