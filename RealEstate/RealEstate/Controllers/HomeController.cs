using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;
using RealEstate.ViewModels;

namespace RealEstate.Controllers;

public class HomeController : Controller
{
    private readonly UserManager <AppUser> _userManager;
     private readonly RealEstateContext _context;

    public HomeController(UserManager<AppUser> userManager,RealEstateContext context)
    {
         _context = context;
        _userManager = userManager;
    }

public ActionResult AdForwarding()
{
    return View();
}
    public async Task  <IActionResult> Index( )
    {
     
        var house = await _context.Houses
        .Where(h => h.Active == "true")
        .Take(8)
        .ToListAsync();
        
         var plot = await _context.Plots
        .Where(h => h.Active == "true")
        .Take(8)
        .ToListAsync();

        
        var viewmodel = new RealEstateViewModel{
            
            HouseList = house,
            PlotList = plot
        };
        return View(viewmodel);
    }


 
}
