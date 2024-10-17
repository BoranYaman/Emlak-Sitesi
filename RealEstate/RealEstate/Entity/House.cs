using RealEstate.Models;

namespace RealEstate.Entity
{
    public class House
    {
        public Guid AdNumber { get; set; }
        public int HouseId { get; set;}
        public string? AdType { get; set;}
        public string? AdStatus { get; set;}
        public string? AdTitle { get; set; }
        public string? Notification { get; set; }
        public string? HouseType { get; set;}
        public DateTime? AnnouncementDate { get; set; }       
        public string? Price { get; set;}

        public string? m2b { get; set;}
        public string? m2n { get; set;}
        public string? NumberOfRooms { get; set;}
        public string? BuildingHistory { get; set;}
        public string? NumberOfFloors { get; set;}
        public string? Floor { get; set;}
        public string? Heating { get; set;}
        public string? Elevator { get; set;}
        public string? CarPark { get; set;}
        public string? Dues { get; set;}
        public string? Explanation { get; set;}
        public string? Country { get; set;}
        public string? Province { get; set;}
        public string? County { get; set;}
        public string? Neighborhood { get; set;}
        public string? Street { get; set;}
         public string? Active { get; set; } 
         public string? PromotionalPicture { get; set; } 

        public string? AppUserId { get; set; } 
         public AppUser? AppUser { get; set; } 
            
        public ICollection<HouseImg>? HouseImages { get; set; }



    }
}