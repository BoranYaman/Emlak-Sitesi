using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Entity;
using RealEstate.Models;
using RealEstate.ViewModels;
using RealEstate.ViewModels.Plot;
using Microsoft.EntityFrameworkCore;

public class RealEstateController : Controller
{
  private readonly RealEstateContext _context;

    public  RealEstateController
    (
         RealEstateContext context,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager
        ){
              {
            _context = context;
            
        }}

    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new HousePlotViewModel()); 
        }

        var houses = await _context.Houses
            .Where(h => h.Province.Contains(query) || h.County.Contains(query) || h.Neighborhood.Contains(query) || h.AdTitle.Contains(query)|| h.AdType.Contains(query)|| h.HouseType.Contains(query))
              .Where(p => p.Active == "true")
            .ToListAsync();

        var plots = await _context.Plots
            .Where(p => p.Province.Contains(query) || p.County.Contains(query) || p.Neighborhood.Contains(query) || p.PlotTitle.Contains(query) || p.PlotType.Contains(query)|| p.AdType.Contains(query))
            .Where(p => p.Active == "true")
            .ToListAsync();

        var result = new HousePlotViewModel
        {
            Houses = houses,
            Plots = plots
        };

        return View(result);
    }
}

internal class ApplicationDbContext
{
}