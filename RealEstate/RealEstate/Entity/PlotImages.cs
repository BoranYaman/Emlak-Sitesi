using System.ComponentModel.DataAnnotations;

namespace RealEstate.Entity
{
    public class PlotImages
{
     [Key] 
    public int PlotImgId { get; set; }
    public string? ImgUrl { get; set; }

    public int PlotId { get; set; }
    public Plot? Plot { get; set; }
}
}