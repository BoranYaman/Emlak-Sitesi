using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;
using RealEstate.ViewModels;
using RealEstate.ViewModels.Role;
using RealEstate.ViewModels.User;



namespace RealEstate.Controllers
{
    public class RoleController: Controller
    {
        private readonly RoleManager <AppRole> _roleManager;
        private readonly UserManager <AppUser> _userManager;
        
        public RoleController (RoleManager <AppRole> roleManager , UserManager <AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager  = userManager;
        }


        [HttpGet]
        public async Task <IActionResult> RoleCreate(UserViewModel model)
        {

             var user = await _userManager.GetUserAsync(User);
        
            if(user == null)
            {
                return NotFound();
            }
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(AppRole model)
        {
           
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleOperations","Role");

                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
                               return RedirectToAction("RoleOperations","Role");

        }
        public async Task<IActionResult> RoleOperations()
        {
            var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                    return RedirectToAction("Error","Error");
            }
            return View(_roleManager.Roles);
        }
        

        
            public async Task<IActionResult> RoleDetails(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error", "Error");
            }

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

            var viewModel = new RoleDetailsViewModel
            {
                Role = role,
                Users = usersInRole.ToList()
            };

            return View(viewModel);
        }


         public async Task <IActionResult>RoleDelete (string id)
         {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();

            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("RoleOperations","Role");
            }
            
            return View();

         }

           public async Task<IActionResult> RoleUpdate(string id)
           
        {
             var user = await _userManager.GetUserAsync(User);
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("Error","Error");
            }
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null && role.Name != null)
            {
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                return View(role);
            }

            return RedirectToAction("RoleOperations");
        }


         
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(AppRole model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("RoleOperations");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name=null!);
                }
            }

            return View(model);
        }
    }
}