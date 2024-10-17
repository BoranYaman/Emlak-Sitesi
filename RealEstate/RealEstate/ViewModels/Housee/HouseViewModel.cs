using RealEstate.Models;
using RealEstate.Entity;
using System.ComponentModel.DataAnnotations;
namespace RealEstate.ViewModels.Housee
{
 public class HouseViewModel
{
     public int HouseId { get; set;}
      public Guid AdNumber { get; set; }
    public string? AdType { get; set; }
    public string? HouseType { get; set; }

    [MaxLength(70, ErrorMessage = "İlan başlığı en fazla 60 karakter olmalıdır.")]
    public string? AdTitle { get; set; }
 public DateTime? AnnouncementDate { get; set; } 
    public string? Price { get; set; }
    public string? m2b { get; set; }
    public string? m2n { get; set; }
    public string? AdStatus { get; set; }
    public string? NumberOfRooms { get; set; }
    public string? BuildingHistory { get; set; }
    public string? NumberOfFloors { get; set; }
    public string? Floor { get; set; }
    public string? Heating { get; set; }
    public string? Elevator { get; set; }
    public string? CarPark { get; set; }
    public string? Dues { get; set; }
    public string? Explanation { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? County { get; set; }
    public string? Neighborhood { get; set; }
    public string? Street { get; set; }
    public string? Active { get; set; }
 public IFormFile? PromotionalPicture { get; set; }
  public AppUser? AppUser { get; set; } 
    

    public List<IFormFile>? Images { get; set; } 
}
}