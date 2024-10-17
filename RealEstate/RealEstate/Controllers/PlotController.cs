using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Entity;
using RealEstate.Models;
using RealEstate.ViewModels;
using RealEstate.ViewModels.Plot;
using Microsoft.EntityFrameworkCore;
using RealEstate.ViewModels.Plot;


namespace RealEstate.Controllers
{
    public class PlotController : Controller
    {
        private readonly RealEstateContext _context;
         private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
    public  PlotController
    (
         RealEstateContext context,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager
        ){
              {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }}
         public async Task<IActionResult>PlotAdd()
        {
             var user = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View();
        }

[HttpPost]
[Authorize]
public async Task<IActionResult> PlotAdd(PlotCreateViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = await _userManager.GetUserAsync(User);
        var plot = new Plot
        {
            AdNumber = Guid.NewGuid(),
            PlotType = model.PlotType,
            PlotPrice = model.PlotPrice,
            PlotTitle = model.PlotTitle,
            M2 = model.M2,
            M2Price = model.M2Price,
            TitleDeedStatus = model.TitleDeedStatus,
            IslandNumber = model.IslandNumber,
            ParcelNumber = model.ParcelNumber,
            Country = model.Country,
            Province = model.Province,
            AnnouncementDate = DateTime.Now,
            Active = "newAd",
            PlotStatus = model.PlotStatus,
            County = model.County,
            AdType = model.AdType,
            Explanation = model.Explanation,
            Neighborhood = model.Neighborhood,
            Street = model.Street,
            CreditEligibility = model.CreditEligibility,
            PromotionalPicture = model.PromotionalPicture?.FileName,
            PlotImages = new List<PlotImages>(),
            AppUserId = user?.Id
        };
        if (model.plotImages != null && model.plotImages.Any())
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/plotimages");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            foreach (var file in model.plotImages)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    plot.PlotImages.Add(new PlotImages { ImgUrl = fileName });
                }
            }
        }
        _context.Plots.Add(plot);
        await _context.SaveChangesAsync();

        return RedirectToAction("MyAd", "Identity");
    }

    return View(model);
}

public async Task<IActionResult> PlotList()
    {
        
         var model = new PlotFilterViewModel
        {
           PlotList = _context.Plots.ToList()
        };

        return View(model);
    }
     [HttpPost]
    public IActionResult Filter(PlotFilterViewModel model)
    {
        var filteredPlots = _context.Plots.AsQueryable();

        if (!string.IsNullOrEmpty(model.County))
        {
            filteredPlots = filteredPlots.Where(h => h.County == model.County);
        }

        if (!string.IsNullOrEmpty(model.PriceRange))
        {
            var priceRange = model.PriceRange.Split('-');
            if (priceRange.Length == 2)
            {
                if (decimal.TryParse(priceRange[0], out var minPrice) && decimal.TryParse(priceRange[1], out var maxPrice))
                {
                    filteredPlots = filteredPlots.Where(h => decimal.Parse(h.PlotPrice) >= minPrice && decimal.Parse(h.PlotPrice) <= maxPrice);
                }
            }
        }

        if (!string.IsNullOrEmpty(model.County))
        {
            filteredPlots = filteredPlots.Where(h => h.County == model.County);
        }

        if (!string.IsNullOrEmpty(model.TitleDeedStatus))
        {
            filteredPlots = filteredPlots.Where(h => h.TitleDeedStatus == model.TitleDeedStatus);
        }

        if (!string.IsNullOrEmpty(model.CreditEligibility))
        {
            filteredPlots = filteredPlots.Where(h => h.CreditEligibility == model.CreditEligibility);
        }
           if (!string.IsNullOrEmpty(model.AdType))
        {
            filteredPlots = filteredPlots.Where(h => h.AdType == model.AdType);
        }

        model.PlotList = filteredPlots.ToList();

        return View("PlotList", model);
    }
    

public async Task<IActionResult> PlotDetails(int id)
{
    var plot = await _context.Plots
                              .Include(p => p.PlotImages)
                              .Include(p => p.AppUser) 
                              .FirstOrDefaultAsync(p => p.PlotId == id);
                              

    if (plot == null)
    {
        return NotFound();
    }

    var viewModel = new RealEstateViewModel
    {
        Plot = plot,
        PlotImagess = plot.PlotImages?.ToList()
    };

    return View(viewModel);
}
 
 [HttpGet]
 [Authorize]
 public async Task<IActionResult> PlotEdit(int id)
 {
    var plot = await _context.Plots.FindAsync(id);
    if (plot == null)
    {
        return NotFound();
    }
    var model = new PlotEditViewModel
    {
        AdNumber = Guid.NewGuid(),
            PlotType = plot.PlotType,
            PlotPrice = plot.PlotPrice,
            PlotTitle = plot.PlotTitle,
            M2 = plot.M2,
            M2Price = plot.M2Price,
            TitleDeedStatus = plot.TitleDeedStatus,
            IslandNumber = plot.IslandNumber,
            ParcelNumber = plot.ParcelNumber,
            Country = plot.Country,
            Province = plot.Province,
            Active = plot.Active,
            PlotStatus = plot.PlotStatus,
            County = plot.County,
            AdType = plot.AdType,
            Explanation = plot.Explanation,
            Neighborhood = plot.Neighborhood,
            Street = plot.Street,
            CreditEligibility = plot.CreditEligibility,
            
    };
    return View(model);
 }
[HttpPost]
[Authorize]
public async Task<IActionResult> PlotEdit(int id , PlotEditViewModel model)
{
    var plot = await _context.Plots.FindAsync(id);
    if (plot == null)
    {
        return NotFound();
    }
        if(ModelState.IsValid)
        {
            plot.PlotTitle = model.PlotTitle;
             plot.PlotType = model.PlotType;
             plot.PlotPrice = model.PlotPrice;
             plot.PlotTitle = model.PlotTitle;
             plot.M2 = model.M2;
             plot.M2Price = model.M2Price;
             plot.TitleDeedStatus = model.TitleDeedStatus;
             plot.IslandNumber = model.IslandNumber;
             plot.ParcelNumber = model.ParcelNumber;
             plot.Country = model.Country;
             plot.Province = model.Province;
             plot.Active ="newAd";
             plot.PlotStatus = model.PlotStatus;
             plot.County = model.County;
             plot.AdType = model.AdType;
             plot.Explanation = model.Explanation;
             plot.Neighborhood = model.Neighborhood;
             plot.Street = model.Street;
             plot.CreditEligibility = model.CreditEligibility;
             
             _context.Plots.Update(plot);
             await _context.SaveChangesAsync();
             return RedirectToAction("MyAd","Identity");
        }
        return View(model);
}
    
    }
}