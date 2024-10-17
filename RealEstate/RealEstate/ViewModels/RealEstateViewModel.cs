using RealEstate.Entity;
using RealEstate.Models;

namespace RealEstate.ViewModels

{
    public class RealEstateViewModel
    {
        public List<AppUser>? Users { get; set; }
            public AppUser User { get; set; }

         public List<IFormFile>? Images { get; set; } 

        public House? House { get; set; }
        public List<House>? HouseList { get; set; }
        public List<HouseImg>? HouseImages { get; set; }
        public List<PlotImages>? PlotImagess { get; set; }

        public RealEstate.Entity.Plot? Plot { get; set; }
        public List <RealEstate.Entity.Plot>? PlotList { get; set; }

    }
}