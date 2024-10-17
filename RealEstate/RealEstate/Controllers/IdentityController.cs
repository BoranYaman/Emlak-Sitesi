using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RealEstate.Models;
using RealEstate.ViewModels;
using RealEstate.ViewModels.User;

using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstate.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        private IEmailSender _emailSender;
        private readonly RealEstateContext _context;
        private SignInManager<AppUser> _signInManager;
        public IdentityController (
            RealEstateContext context,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,

        SignInManager<AppUser> signInManager,
        IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }
 
        [HttpPost]
        public async Task <IActionResult> Login(LoginViewModel model, UserViewModel model2)
        {
            var user2 = await _userManager.GetUserAsync(User);

            

            if (ModelState.IsValid) 
            {
                var user  = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                if(!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Lütfen E Postanızı onaylayın");
                }
                var result = await _signInManager.PasswordSignInAsync(user,model.Password, model.RememberMe,true);
                 if(user.AccountStatus=="Kapalı")
                    {
                        return RedirectToAction("LoginRedirect", "Identity");
                    }
                else if (result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
            }
            else {
                ModelState.AddModelError("", "Hatalı Bilgi");
            }
            }

            return View(model);
        }
        public IActionResult LoginRedirect()
        {
            return View();
        }

         public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address ?? "",
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "Identity", new { user.Id, token });
                        if (string.IsNullOrEmpty(url))
                        {
                            throw new Exception("URL oluşturulamadı.");
                        }
                   await _emailSender.SendEmailAsync(user.Email, "E-posta Onayı", $"Lütfen E-posta Hesabınızı Onaylamak İçin Linke <a href='http://localhost:5113{url}'> Tıklayınız </a>");

                    TempData ["message"] = "Hesabınızı Onaylayın";

                    return RedirectToAction("Login", "Identity");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            if (Id == null || token == null)
            {
                TempData ["message"] = "Geçersiz Token Bilgisi";
                return View();
            }
            var user = await _userManager.FindByIdAsync(Id);
            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user,token);
                if (result.Succeeded)
                {
                    TempData ["message"] = "Hesabınız Onaylandı";
                return View();
                }
            }
            TempData ["message"] = "Böyle Bir kullanıcı yok";
                return View ();


        }

          [Authorize]        
          public async Task <IActionResult>MyAccount()
        {

          
            var user = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
               
            }
            else
            {
                return RedirectToAction("Login", "Identity");
            }
            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user !);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = new UserEditViewModel
            {
              
                UserName = user.UserName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,


            };

                return View(model);
        }
        
          [Authorize]
            public async Task<IActionResult> UserOperations(string searchEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchEmail))
            {
                users = users.Where(u => u.Email!.Contains(searchEmail));
            }

            var userList = await users.ToListAsync();

            var userViewModel = new UserViewModel
            {
                Users = userList
            };

            ViewData["searchEmail"] = searchEmail;

            return View(userViewModel);
        }

              [Authorize]
            public async Task<IActionResult>AccountDetails(string id)
            {
                var users = await _userManager.GetUserAsync(User);
                if(!User.IsInRole("Admin"))
                {
                    return RedirectToAction("Error","Error");
                }
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);

            }

            [HttpGet]
            [Authorize]
            
            public async Task<IActionResult> UserAddRole(string id, UserEditViewModel model)
        {

            var user2 = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
            

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(id);
         

            if (user != null)
            {
                ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

                return View(new UserEditViewModel
                {
                    UserName = user.UserName,
                    LastName = user.LastName,
                    Email=user.Email,
                });
            }

            return RedirectToAction("Index");
        }

       
          [HttpGet]
            [Authorize]
        public async Task<IActionResult> AccountTransactions(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Error", "Error");
            }

            var model = new UserEditViewModel
            {
                UserName = user.UserName,
                LastName = user.LastName,
                Email = user.Email,
                AccountStatus = user.AccountStatus=null!,
                ReasonForObstacle = user.ReasonForObstacle=null!
            };

            return View(model);
        }

        [HttpPost]
          [Authorize]
        public async Task<IActionResult> AccountTransactions(string id, UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user != null)
                {
                    user.AccountStatus = model.AccountStatus;
                    user.ReasonForObstacle = model.ReasonForObstacle;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("UserOperations", "Identity");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return View(model);
        }

        
        public async Task<IActionResult> OtherAds(string userId)
{
    var userHouses = await _context.Houses
                                   .Where(h => h.AppUserId == userId)
                                   .Where(h => h.Active == "true") 

                                   .Include(h => h.HouseImages)
                                   .Include(h => h.AppUser) 
                                   .ToListAsync();
    var userPlot = await _context.Plots
                                   .Where(h => h.AppUserId == userId) 
                                   .Where(h => h.Active == "true") 

                                   .Include(h => h.PlotImages)
                                   .Include(h => h.AppUser) 
                                   .ToListAsync();


    if (userHouses == null || userHouses.Count == 0)
    {
        return NotFound();
    }

    var viewModel = new RealEstateViewModel
    {
        PlotList = userPlot,
        HouseList = userHouses
    };

    return View(viewModel);
}

  [Authorize]
public async Task<IActionResult> MyAd(string userId)
{
    var user = await _userManager.GetUserAsync(User);
    
    if (user == null)
    {
        return RedirectToAction("Login", "Account"); 
    }

    var userHouses = await _context.Houses
                                   .Where(h => h.AppUserId == user.Id)
                                   .Include(h => h.HouseImages)
                                   .ToListAsync();

    var userPlots = await _context.Plots
                                  .Where(p => p.AppUserId == user.Id)
                                  .Include(p => p.PlotImages)
                                  .ToListAsync();

    var viewModel = new RealEstateViewModel
    {
        HouseList = userHouses,
        HouseImages = userHouses.SelectMany(h => h.HouseImages).ToList(),
        PlotList = userPlots,
        PlotImagess = userPlots.SelectMany(p => p.PlotImages).ToList() 
    };

    return View(viewModel);
}

[Authorize]
     public async Task<IActionResult> MyAdDetails(int id)
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

[Authorize]
public async Task<IActionResult> MyPlotAdDetails(int id)
{
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



    }
    }

