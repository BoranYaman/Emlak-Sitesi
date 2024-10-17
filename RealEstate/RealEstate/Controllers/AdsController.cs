using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;

namespace RealEstate.Controllers
{
    public class AdsController : Controller
    {
      private  readonly  UserManager <AppUser> _userManager;
        public AdsController(UserManager <AppUser> userManager)
        {
            _userManager = userManager;
        }
          [Authorize]
        public  async Task <IActionResult> MyAds()
        { 
            var user = await _userManager.GetUserAsync(User);
            if(!await _userManager.IsEmailConfirmedAsync(user!))
            {
                return RedirectToAction("AdsError","Error");
            }
            return View();
        }

        
    }
}