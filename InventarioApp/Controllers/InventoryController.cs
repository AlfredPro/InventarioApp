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
        public async Task<IActionResult> Index()
        {
            return _context.InventoryEntry != null ? 
                          View(await _context.InventoryEntry.ToListAsync()) :
                          Problem("Entity set 'AppDBContext.InventoryEntry'  is null.");
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
