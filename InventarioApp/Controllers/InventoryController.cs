﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventarioApp.Areas.Identity.Data;
using InventarioApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace InventarioApp.Controllers
{
    [Authorize(Roles = "Worker,MafiaBoss")]
    public class InventoryController : Controller
    {
        private readonly AppDBContext _context;

        public InventoryController(AppDBContext context)
        {
            _context = context;
        }

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

            if(page != null && page <1) page = 1;

            return View(await PaginatedList<InventoryEntry>.CreateAsync(entries.AsNoTracking(), page ?? 1, 10));
                          
        }

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

        public IActionResult Create()
        {
            return View();
        }

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
