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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _config;

        public UsersController(ApplicationDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        // [HttpPost]
        // public IActionResult Login(Users user)
        // {
        //     var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

        //     if (existingUser == null)
        //     {
        //         return View("IncorrectCredentials");
        //     }

        //     string hashedPasswordFromDB = existingUser.Password;
        //     string hashedPassword = PasswordHasher.HashPassword(user.Password);

        //     if (hashedPassword != hashedPasswordFromDB)
        //     {
        //         return View("IncorrectCredentials");
        //     }

        //     var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //     var Sectoken = new JwtSecurityToken(
        //         _config["Jwt:Issuer"],
        //         _config["Jwt:Issuer"],
        //         new List<Claim>
        //         {
        //             new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
        //             new Claim(ClaimTypes.GivenName, existingUser.FirstName),
        //             new Claim(ClaimTypes.Surname, existingUser.LastName),
        //         },
        //         expires: DateTime.Now.AddHours(2),
        //         signingCredentials: credentials
        //     );

        //     var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

        //     Response.Cookies.Append("jwtToken", token, new CookieOptions
        //     {
        //         HttpOnly = true,
        //         // Other cookie settings
        //     });

        //     return RedirectToAction("Index", "Urls");
        // }

        [Authorize]
        // GET: Users
        public async Task<IActionResult> Index()
        {
            var Users = await _context.Users.ToListAsync();

            return View(Users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var user = new Users
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,FirstName,LastName,IsAdmin,CreatedDate,UpdatedDate")] Users Users)
        {
            if (ModelState.IsValid)
            {
                Users.Password = PasswordHasher.HashPassword(Users.Password);

                _context.Add(Users);
                await _context.SaveChangesAsync();

                return View("ConfirmedLogin");
            }

            return View(Users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users.FindAsync(id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,FirstName,LastName,IsAdmin,CreatedDate,UpdatedDate")] Users Users)
        {
            if (id != Users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(Users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var Users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var Users = await _context.Users.FindAsync(id);
            if (Users != null)
            {
                _context.Users.Remove(Users);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
