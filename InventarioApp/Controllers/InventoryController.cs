using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventarioApp.Areas.Identity.Data;
using InventarioApp.Models;

namespace InventarioApp.Controllers
{
    public class InventoryController : Controller
    {
        private readonly AppDBContext _context;

        public InventoryController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index(string searchForString, string searchByString, int? page)
        {
            if (_context.InventoryEntry == null)
            {
                return Problem("No DBContext found");
            }
            if(searchForString == null || searchByString == null)
            {
                searchForString = "";
                searchByString = "";
            }
            ViewData["searchForString"] = searchForString;
            ViewData["searchByString"] = searchByString;

            ViewBag.Options = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nombre", Value = "Name" },
                new SelectListItem { Text = "Tipo", Value = "Type" },
                new SelectListItem { Text = "Descripcion", Value = "Description" },
                new SelectListItem { Text = "Notas", Value = "Notes" }
            };

            var entries = from s in _context.InventoryEntry
                          select s;

            if (!String.IsNullOrEmpty(searchForString))
            {
                switch (searchByString)
                {
                    case "Name":
                        entries = entries.Where(s => s.Name.ToLower().Contains(searchForString.ToLower()));
                        break;
                    case "Type":
                        entries = entries.Where(s => s.Type.ToLower().Contains(searchForString.ToLower()));
                        break;
                    case "Description":
                        entries = entries.Where(s => s.Description.ToLower().Contains(searchForString.ToLower()));
                        break;
                    case "Notes":   
                        entries = entries.Where(s => s.Notes.ToLower().Contains(searchForString.ToLower()));
                        break;
                    default:
                        break;
                }
            }

            entries = entries.OrderBy(s => s.Type).ThenBy(s => s.Name);
            return View(await PaginatedList<InventoryEntry>.CreateAsync(entries.AsNoTracking(), page ?? 1, 10));
                          
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InventoryEntry == null)
            {
                return NotFound();
            }

            var inventoryEntry = await _context.InventoryEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryEntry == null)
            {
                return NotFound();
            }

            return View(inventoryEntry);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Description,Notes,Quantity")] InventoryEntry inventoryEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryEntry);
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InventoryEntry == null)
            {
                return NotFound();
            }

            var inventoryEntry = await _context.InventoryEntry.FindAsync(id);
            if (inventoryEntry == null)
            {
                return NotFound();
            }
            return View(inventoryEntry);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Description,Notes,Quantity")] InventoryEntry inventoryEntry)
        {
            if (id != inventoryEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryEntryExists(inventoryEntry.Id))
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
            return View(inventoryEntry);
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InventoryEntry == null)
            {
                return NotFound();
            }

            var inventoryEntry = await _context.InventoryEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryEntry == null)
            {
                return NotFound();
            }

            return View(inventoryEntry);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InventoryEntry == null)
            {
                return Problem("Entity set 'AppDBContext.InventoryEntry'  is null.");
            }
            var inventoryEntry = await _context.InventoryEntry.FindAsync(id);
            if (inventoryEntry != null)
            {
                _context.InventoryEntry.Remove(inventoryEntry);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryEntryExists(int id)
        {
          return (_context.InventoryEntry?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
