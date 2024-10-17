namespace RealEstate.Entity
{
    public class HouseImg
{
    public int HouseImgId { get; set; }
    public string? ImgUrl { get; set; }
    public int HouseId { get; set; }  
    public House? House { get; set; } 
}
}