namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class LocalIdentityUser :  IdentityUser
{
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

}
