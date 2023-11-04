using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestTask.Context;
using TestTask.Models;
using TestTask.Utils;
using Microsoft.AspNetCore.Authorization;


namespace TestTask.Controllers
{
    public class UrlsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UrlsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Urls

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwtToken"];
            // Console.WriteLine("AZAZA token ", token);
            if (token != null) {
                ViewBag.currentUser = ParserJWT.parse(token);
            }

            return _context.Urls != null ?
                        View(await _context.Urls.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Urls'  is null.");
        }

        // GET: Urls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Urls == null)
            {
                return NotFound();
            }

            var Urls = await _context.Urls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Urls == null)
            {
                return NotFound();
            }

            return View(Urls);
        }

        // GET: Urls/Create
        public IActionResult Create()
        {
            var token = Request.Cookies["jwtToken"];
            if (token == null) {
                return RedirectToAction("Login", "Auth");
            }

            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,OriginalUrl,ShortUrl,CreatedDate,UpdatedDate")] Urls Urls)
        {
            var token = Request.Cookies["jwtToken"];
            if (token == null) {
                return RedirectToAction("Login", "Auth");
            }
            var currentUser = ParserJWT.parse(token);

            Urls.ShortUrl = ShortStringGenerator.GenerateShortUrl();
            Urls.UserId = currentUser.userId;
            Urls.CreatedDate = DateTime.Now;
            Urls.UpdatedDate = DateTime.Now;

            _context.Add(Urls);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Urls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var token = Request.Cookies["jwtToken"];
            if (token == null) {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null || _context.Urls == null)
            {
                return NotFound();
            }

            var Urls = await _context.Urls.FindAsync(id);
            if (Urls == null)
            {
                return NotFound();
            }
            return View(Urls);
        }

        // POST: Urls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,OriginalUrl,ShortUrl,CreatedDate,UpdatedDate")] Urls Urls)
        {
            if (id != Urls.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Urls);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrlsExists(Urls.Id))
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
            return View(Urls);
        }

        // GET: Urls/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Urls == null)
            {
                return NotFound();
            }

            var Urls = await _context.Urls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Urls == null)
            {
                return NotFound();
            }

            return View(Urls);
        }

        // POST: Urls/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Urls == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Urls'  is null.");
            }
            var Urls = await _context.Urls.FindAsync(id);
            if (Urls != null)
            {
                _context.Urls.Remove(Urls);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UrlsExists(int id)
        {
            return (_context.Urls?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
