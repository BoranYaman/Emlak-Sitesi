using RealEstate.Entity;

namespace RealEstate.ViewModels.Plot
{
    public class PlotFilterViewModel
    {  public string? County { get; set; }
        public string? PriceRange { get; set; }
        public string? CreditEligibility { get; set; }
        public string? TitleDeedStatus { get; set; }
        public string? AdType { get; set; }

              public List <RealEstate.Entity.Plot>? PlotList { get; set; }


    }
}