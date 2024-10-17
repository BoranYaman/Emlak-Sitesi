using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;
using RealEstate.ViewModels;
using RealEstate.ViewModels.Housee;
using RealEstate.ViewModels.Plot;

namespace RealEstate.Controllers{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly RealEstateContext _context;

        public AdminController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            RealEstateContext context)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _context = context;
            }
            
       public ActionResult AdminAdControlPanel()
       {
        return View();
       }
       
       public async Task<IActionResult>HouseAdApproval()
       {
         var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
        var house = await _context.Houses
        .Where(h => h.Active=="false" || h.Active=="newAd")
        .ToListAsync();
        return View(house);
      
       }

       public async Task<IActionResult>PlotAdApproval()
       { 
          var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
        var plot = await _context.Plots
        .Where(p => p.Active=="newAd" || p.Active=="false")
        .ToListAsync();
        return View(plot);
       }


       public async Task<IActionResult> ApprovedHouse()
       {
          var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
        var house = await _context.Houses
        .Where(h => h.Active == "true")
        .ToListAsync();
        return View(house);
       }

       public async Task <IActionResult>ApprovedPlot()
       {  var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
        var plot = await _context.Plots
        .Where(p => p.Active == "true")
        .ToListAsync();
        return View(plot);
       }


        public async Task<IActionResult> HouseAdApprovalUp(int id)
{
     var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
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
     public async Task<IActionResult> PlotAdApprovalUp(int id)
{
     var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
    var plot = await _context.Plots
                              .Include(h => h.PlotImages)
                              .Include(h => h.AppUser) 
                              .FirstOrDefaultAsync(h => h.PlotId == id);
                              

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
public async Task<IActionResult>HomeTransactionForm(int id)
{
     var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
    var house = await _context.Houses.FindAsync(id);
    if (house == null)
    {
        return NotFound();
    }

    var model = new HouseEditViewModel
    {
        
        Active = house.Active,
        AdStatus = house.AdStatus,

    };
    return View(model);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> HomeTransactionForm(int id, HouseEditViewModel model)
{
     var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
    var house = await _context.Houses.FindAsync(id);
    if (house == null)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
       house.AdStatus = model.AdStatus;
       house.Active = model.Active;
       
        _context.Houses.Update(house);
        await _context.SaveChangesAsync();
        return RedirectToAction("AdminAdControlPanel", "Admin");
    }

    return View(model);
}

       [HttpGet]
  [Authorize]
public async Task<IActionResult>PlotTransactionForm(int id)
{
     var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
    var plot = await _context.Plots.FindAsync(id);
    if (plot == null)
    {
        return NotFound();
    }

    var model = new PlotEditViewModel
    {
        Active = plot.Active,
        PlotStatus = plot.PlotStatus,

    };
    return View(model);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> PlotTransactionForm(int id, PlotEditViewModel model)
{
     var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
    var plot = await _context.Plots.FindAsync(id);
    if (plot == null)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
       plot.PlotStatus = model.PlotStatus;
       plot.Active = model.Active;
       
        _context.Plots.Update(plot);
        await _context.SaveChangesAsync();
        return RedirectToAction("AdminAdControlPanel", "Admin");
    }

    return View(model);
}

    }
}