using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;
using TestTask.Models;
using System.Security.Claims;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Auth");
        }

        // public IActionResult IncorrectCredentials()
        // {
        //     return View();
        // }


        // public IActionResult Urls()
        // {
        //     ViewBag.currentUser = this.getCurrentUser();

        //     return View();
        // }

        // public IActionResult About()
        // {
        //     ViewBag.currentUser = this.getCurrentUser();

        //     return View();
        // }

        // public IActionResult Users()
        // {
        //     ViewBag.currentUser = this.getCurrentUser();

        //     return View();
        // }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }

        private object getCurrentUser()
        {
            var jwtToken = Request.Cookies["jwtToken"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            var currentUser = new
                {
                    userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    firstName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                    lastName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                };

            return currentUser;
        }
    }
}