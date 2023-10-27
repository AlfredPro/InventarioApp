using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventarioApp.Areas.Identity.Data;
using InventarioApp.Models;
using Microsoft.AspNetCore.Identity;

namespace InventarioApp.Controllers
{
    public class UserManagementsController : Controller
    {
        private readonly AppDBContext _context;

        public UserManagementsController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchForString, string searchByString, int? page)
        {
            if(_context.Users == null)
            {
                return Problem("No DBContext found");
            }
            if (searchForString == null || searchByString == null)
            {
                searchForString = "";
                searchByString = "";
            }
            ViewData["searchForString"] = searchForString;
            ViewData["searchByString"] = searchByString;

            ViewBag.Options = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nombre", Value = "FirstName" },
                new SelectListItem { Text = "Apellido", Value = "LastName" },
                new SelectListItem { Text = "Correo", Value = "Email" },
                new SelectListItem { Text = "Rol", Value = "Role" }
            };

            var users = (from u in _context.Users
                         join ur in _context.UserRoles on u.Id equals ur.UserId
                         join r in _context.Roles on ur.RoleId equals r.Id into roles
                         select new UserManagement
                         {
                             Id = u.Id,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             Email = u.Email ?? "",
                             Role = roles.ToList().First().Name ?? "Noob"
                         });

            if (!String.IsNullOrEmpty(searchForString))
            {
                switch (searchByString)
                {
                    case "FirstName":
                        users = users.Where(s => s.FirstName.ToLower().Contains(searchForString.ToLower()));
                        break;
                    case "LastName":
                        users = users.Where(s => s.LastName.ToLower().Contains(searchForString.ToLower()));
                        break;
                    case "Email":
                        users = users.Where(s => s.Email.ToLower().Contains(searchForString.ToLower()));
                        break;
                    case "Role":
                        users = users.Where(s => s.Role.ToLower().Contains(searchForString.ToLower()));
                        break;
                    default:
                        break;
                }
            }

            users = users.OrderBy(s => s.LastName).ThenBy(s => s.FirstName);

            if (page != null && page < 1) page = 1;

            return View(await PaginatedList<UserManagement>.CreateAsync(users.AsNoTracking(), page ?? 1, 3));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userManagement = await _context.Users.FindAsync(id);
            if (userManagement == null)
            {
                return NotFound();
            }
            return View(userManagement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Email,RoleId")] UserManagement userManagement)
        {
            if (id != userManagement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userManagement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserManagementExists(userManagement.Id))
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
            return View(userManagement);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userManagement = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userManagement == null)
            {
                return NotFound();
            }

            return View(userManagement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'AppDBContext.UserManagement'  is null.");
            }
            var userManagement = await _context.Users.FindAsync(id);
            if (userManagement != null)
            {
                _context.Users.Remove(userManagement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserManagementExists(string id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
