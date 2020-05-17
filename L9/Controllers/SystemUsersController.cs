using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using L9.Models;
using Microsoft.AspNetCore.Http;

namespace L9.Controllers
{
    public class SystemUsersController : Controller
    {
        private readonly Ramadan2020Context ORM;

        public SystemUsersController(Ramadan2020Context context)
        {
            ORM = context;
        }

        // GET: SystemUsers
        public async Task<IActionResult> Index()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Role")))
            {
                return RedirectToAction("Login");
            }

            return View(await ORM.SystemUser.ToListAsync());
        }

        // GET: SystemUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await ORM.SystemUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }


        // GET: SystemUsers/Login
        public IActionResult Login()
        {
            return View();
        }


        // POST: SystemUsers/Login
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {

SystemUser LoggedInUser =             ORM.SystemUser.Where(a => a.Username == Username && a.Password == Password).FirstOrDefault();


            if(LoggedInUser ==null)
            {
                //for custom error messaging
                //ViewBag.Message = "Invalid Details";


                ModelState.AddModelError("", "Invalid Details, Please try again.");
                return View();
            }

            HttpContext.Session.SetString("Role",LoggedInUser.Role);
            HttpContext.Session.SetString("DisplayName", LoggedInUser.DisplayName);


            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        // GET: SystemUsers/Create
        public IActionResult Create()
        {
//page leve autho.
            if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("UnAuthroizedAccess");
            }


            return View();
        }

        public IActionResult UnAuthroizedAccess()
        {
            return View();
        }

        // POST: SystemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                ORM.Add(systemUser);
                await ORM.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await ORM.SystemUser.FindAsync(id);
            if (systemUser == null)
            {
                return NotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,Role,DisplayName,Username,Password")] SystemUser systemUser)
        {
            if (id != systemUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ORM.Update(systemUser);
                    await ORM.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemUserExists(systemUser.Id))
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
            return View(systemUser);
        }

        // GET: SystemUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await ORM.SystemUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }

        // POST: SystemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var systemUser = await ORM.SystemUser.FindAsync(id);
            ORM.SystemUser.Remove(systemUser);
            await ORM.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemUserExists(int id)
        {
            return ORM.SystemUser.Any(e => e.Id == id);
        }
    }
}
