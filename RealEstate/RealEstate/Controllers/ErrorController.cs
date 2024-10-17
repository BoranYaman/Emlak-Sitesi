using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Controllers
{
    public class ErrorController:Controller
    {
        public IActionResult Error()
        {
            return View();
        }

         public IActionResult AdsError()
        {
            return View();
        }
        
         public IActionResult  StatusError()
        {
            return View();
        }
    }
}