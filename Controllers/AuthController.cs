using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestTask.Context;
using TestTask.Models;
using TestTask.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TestTask.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(Users user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser == null)
            {
                return View("IncorrectCredentials");
            }

            string hashedPasswordFromDB = existingUser.Password;
            string hashedPassword = PasswordHasher.HashPassword(user.Password);

            if (hashedPassword != hashedPasswordFromDB)
            {
                return View("IncorrectCredentials");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                    new Claim(ClaimTypes.GivenName, existingUser.FirstName),
                    new Claim(ClaimTypes.Surname, existingUser.LastName),
                    new Claim(ClaimTypes.Role, existingUser.IsAdmin == true ? "true" : "false"),
                },
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
            });

            return RedirectToAction("Index", "Urls");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    
    }
}
