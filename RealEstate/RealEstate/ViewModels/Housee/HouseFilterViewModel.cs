using RealEstate.Entity;

namespace RealEstate.ViewModels.Housee
{
    public class HouseFilterViewModel
    {  public string? County { get; set; }
        public string? PriceRange { get; set; }
        public string? NumberOfRooms { get; set; }
        public string? AdType { get; set; }
        public string? CarPark { get; set; }

        public List<House>? HouseList { get; set; }


    }
}