using RealEstate.Entity;

namespace RealEstate.ViewModels.User
{
    public class UserEditViewModel
    {
        public string? UserName { get; set;} = string.Empty;
        public string? LastName { get; set;} = string.Empty;
        public string? Email { get; set;} = string.Empty;
        public string? Address { get; set;} = string.Empty;
        public string? PhoneNumber { get; set;} = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string? Password { get; set;} = string.Empty;
        public string AccountStatus { get; set; } = string.Empty;
        public string ReasonForObstacle { get; set; } = string.Empty;
         public ICollection<House>? Houses { get; set; }

        public IList<string>? SelectedRoles { get; set; }
        
    }
}
