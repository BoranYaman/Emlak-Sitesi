using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstate.Entity;
using RealEstate.Models;
using RealEstate.ViewModels;
using RealEstate.ViewModels.Housee;
using RealEstate.ViewModels.User;


namespace RealEstate.Controllers
{

    public class HouseController : Controller
    {
        private readonly RealEstateContext _context;
        private readonly UserManager<AppUser> _userManager;
        public HouseController(RealEstateContext context,
       UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        public  async Task <IActionResult> HouseAdd(UserViewModel model)
        {
          
                return View();
            
        }
 [HttpPost]
   [Authorize]
public async Task<IActionResult> HouseAdd(HouseViewModel model)
{
    if (ModelState.IsValid)
    {
        
        var currentUser = await _userManager.GetUserAsync(User);

        var house = new House
        {
            AdType = model.AdType,
            AdNumber = Guid.NewGuid(),
            HouseType = model.HouseType,
            Price = model.Price,
            m2b = model.m2b,
            m2n = model.m2n,
            NumberOfRooms = model.NumberOfRooms,
            BuildingHistory = model.BuildingHistory,
            NumberOfFloors = model.NumberOfFloors,
            Floor = model.Floor,
            Heating = model.Heating,
            Elevator = model.Elevator,
            CarPark = model.CarPark,
            AnnouncementDate = DateTime.Now,
            Dues = model.Dues,
            Explanation = model.Explanation,
            Country = model.Country,
            Province = model.Province,
            County = model.County,
            Neighborhood = model.Neighborhood,
            Street = model.Street,
            Active = "newAd",
            AdStatus=model.AdStatus,
            AdTitle = model.AdTitle,
            AppUserId = currentUser?.Id, 
            HouseImages = new List<HouseImg>()
        };

        if (model.PromotionalPicture != null && model.PromotionalPicture.Length > 0)
        {
            var fileName = Path.GetFileName(model.PromotionalPicture.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.PromotionalPicture.CopyToAsync(stream);
            }

            house.PromotionalPicture = $"/images/{fileName}";
        }

        if (model.Images != null && model.Images.Count > 0)
        {
            foreach (var image in model.Images)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    house.HouseImages.Add(new HouseImg
                    {
                        ImgUrl = $"/images/{fileName}"
                    });
                }
            }
        }

        _context.Houses.Add(house);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index","Home"); 
    }

    return View(model);
}

  public async Task<IActionResult> HouseDetails(int id)
{
    var house = await _context.Houses
                              .Include(h => h.HouseImages)
                              .Include(h => h.AppUser) 
                              .FirstOrDefaultAsync(h => h.HouseId == id);
                              

    if (house == null)
    {
        return NotFound();
    }

    var viewModel = new RealEstateViewModel
    {
        House = house,
        HouseImages = house.HouseImages?.ToList()
    };

    return View(viewModel);
}


    public async Task<IActionResult> HouseList()
    {

        
       // İlk sayfa yüklendiğinde tüm evleri getiriyoruz
        var model = new HouseFilterViewModel
        {
            HouseList = _context.Houses.ToList()
        };

        return View(model);
    }
     [HttpPost]
    public IActionResult Filter(HouseFilterViewModel model)
    {
        // Filtreleme işlemi
        var filteredHouses = _context.Houses.AsQueryable();

        if (!string.IsNullOrEmpty(model.County))
        {
            filteredHouses = filteredHouses.Where(h => h.County == model.County);
        }

        if (!string.IsNullOrEmpty(model.PriceRange))
        {
            var priceRange = model.PriceRange.Split('-');
            if (priceRange.Length == 2)
            {
                if (decimal.TryParse(priceRange[0], out var minPrice) && decimal.TryParse(priceRange[1], out var maxPrice))
                {
                    filteredHouses = filteredHouses.Where(h => decimal.Parse(h.Price) >= minPrice && decimal.Parse(h.Price) <= maxPrice);
                }
            }
        }

        if (!string.IsNullOrEmpty(model.NumberOfRooms))
        {
            filteredHouses = filteredHouses.Where(h => h.NumberOfRooms == model.NumberOfRooms);
        }

        if (!string.IsNullOrEmpty(model.AdType))
        {
            filteredHouses = filteredHouses.Where(h => h.AdType == model.AdType);
        }

        if (!string.IsNullOrEmpty(model.CarPark))
        {
            filteredHouses = filteredHouses.Where(h => h.CarPark == model.CarPark);
        }

        // Filtrelenmiş evleri model'e ekliyoruz
        model.HouseList = filteredHouses.ToList();

        return View("HouseList", model);
    }
    

[HttpGet]
  [Authorize]
public async Task<IActionResult>HouseEdit(int id)
{
    var house = await _context.Houses.FindAsync(id);
    if (house == null)
    {
        return NotFound();
    }
    var existingImages = house.HouseImages?.Select(img => img.ImgUrl).ToList();

    var model = new HouseEditViewModel
    {
        AdType = house.AdType,
        HouseType = house.HouseType,
        AdTitle = house.AdTitle,
        Price = house.Price,
        m2b = house.m2b,
        m2n = house.m2n,
        AdStatus = house.AdStatus,
        NumberOfRooms = house.NumberOfRooms,
        BuildingHistory = house.BuildingHistory,
        NumberOfFloors = house.NumberOfFloors,
        Floor = house.Floor,
        Heating = house.Heating,
        Elevator = house.Elevator,
        CarPark = house.CarPark,
        Dues = house.Dues,
        Explanation = house.Explanation,
        Country = house.Country,
        Province = house.Province,
        County = house.County,
        Neighborhood = house.Neighborhood,
        Street = house.Street,

    };
    return View(model);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> HouseEdit(int id, HouseEditViewModel model)
{
    var house = await _context.Houses.FindAsync(id);
    if (house == null)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        house.AdType = model.AdType;
        house.HouseType = model.HouseType;
        house.AdTitle = model.AdTitle;
        house.Price = model.Price;
        house.m2b = model.m2b;
        house.m2n = model.m2n;
        house.AdStatus = model.AdStatus;
        house.NumberOfRooms = model.NumberOfRooms;
        house.BuildingHistory = model.BuildingHistory;
        house.NumberOfFloors = model.NumberOfFloors;
        house.Floor = model.Floor;
        house.Heating = model.Heating;
        house.Elevator = model.Elevator;
        house.CarPark = model.CarPark;
        house.Dues = model.Dues;
        house.Explanation = model.Explanation;
        house.Country = model.Country;
        house.Province = model.Province;
        house.County = model.County;
        house.Neighborhood = model.Neighborhood;
        house.PromotionalPicture = house.PromotionalPicture;
        house.Street = model.Street;
        house.Active = "newAd";

       

        _context.Houses.Update(house);
        await _context.SaveChangesAsync();
        return RedirectToAction("MyAd", "Identity");
    }

    return View(model);
}



}
}