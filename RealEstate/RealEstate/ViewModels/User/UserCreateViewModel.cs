using System.ComponentModel.DataAnnotations;
using RealEstate.Entity;

namespace RealEstate.ViewModels.User
{
    public class UserCreateViewModel
    {
        
        public string UserName { get; set;}=string.Empty;

        public string LastName { get; set;}=string.Empty;
        [Required]

        [EmailAddress]
        public string Email {get; set; }=string.Empty;
        [Required]
        
        public string? Address  {get; set;}=string.Empty;
        [Required]

        [Phone]
        public string? PhoneNumber  { get; set;}=string.Empty;

        [Required]
        [DataType(DataType.Password)]   
        public string Password { get; set; }= string.Empty;
        public string? AccountStatus { get; set; }= string.Empty;
        public string?  ReasonForObstacle { get; set; }= string.Empty;
         public ICollection<House>? Houses { get; set; }
        

       





    }
}