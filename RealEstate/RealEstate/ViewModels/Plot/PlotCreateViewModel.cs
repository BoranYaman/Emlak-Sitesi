using RealEstate.Entity;
using RealEstate.Models;

namespace RealEstate.ViewModels.Plot
{
    public class PlotCreateViewModel
    {
        public int PlotId { get; set; }
        public Guid AdNumber { get; set; }
        public string? PlotStatus { get; set;}
        public string? Active { get; set;}
        public string? Explanation { get; set;}
        public string? AnnouncementDate { get; set;}
        public string? PlotPrice { get; set;}
        public string? PlotTitle { get; set; }
        public string? PlotType { get; set; }
        public string? M2 { get; set; }
        public string? M2Price { get; set; }
        public string? IslandNumber { get; set; }
        public string? ParcelNumber { get; set; }
        public string? CreditEligibility { get; set; }
        public string? TitleDeedStatus { get; set; }
        public string? AdType { get; set; }

        
        public string? Street { get; set;}
        public string? Neighborhood { get; set;}
        public string? County { get; set;}
        public string? Province { get; set;}
        public string? Country { get; set;}
    public string? AppUserId { get; set; } 
         public AppUser? AppUser { get; set; } 
            public List<IFormFile>? Images { get; set; } 
             public IFormFile PromotionalPicture { get; set; } 
    public List<IFormFile>? plotImages { get; set; }   

        

      

    }
}