
using RealEstate.Models;

namespace RealEstate.ViewModels.Role
{

public class RoleDetailsViewModel
{
    public AppRole? Role { get; set; }
    public List<AppUser>? Users { get; set; }
}
}
