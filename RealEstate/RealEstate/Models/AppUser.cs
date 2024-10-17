using Microsoft.AspNetCore.Identity;
using RealEstate.Entity;
namespace RealEstate.Models
{
    public class AppUser:IdentityUser
    {

        public string? LastName  { get; set;}
        public string? AccountStatus { get; set; }
        public string?  ReasonForObstacle { get; set; }
        public string? Address  { get; set;}
         public ICollection<House>? Houses { get; set; }
         

    }
}