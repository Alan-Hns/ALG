using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudSecApp.Models;
using CrudSecApp.Models.UsViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CrudSecApp.Controllers
{
    [Authorize(Roles = "1")]
    public class RolesController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public RolesController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
              return _context.Roles != null ? 
                          View(await _context.Roles.ToListAsync()) :
                          Problem("Entity set 'SecurityApplications_UATContext.Roles'  is null.");
        }

        // GET: DetalleRoles
        public async Task<IActionResult> IndexDR(int? id)
        {

            var viewModel = new ModelUR
            {
                DetatlleRol = await _context.DetalleRols
                    .Include(a => a.ApplicationPermission)
                .Where(a => a.RolId == id)
                .ToListAsync(),
                AppUserRols = await _context.ApplicationUserRols
                .Include(a => a.Roles)
                    .ThenInclude(a => a.DetalleRols)
                        .ThenInclude(a => a.ApplicationPermission)
                .Include(a => a.ApplicationUser)
                    .ThenInclude(a => a.ApplicationUserPermissions)
                        .ThenInclude(a => a.ApplicationPermission)
                .Where(a => a.Roles.Id == id)
                .ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .Include(a => a.DetalleRols)
                    .ThenInclude(a => a.ApplicationPermission)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Descripcion,Estado")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/CreateD
        public IActionResult CreateD()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Codigo");
            ViewData["ApplicationPermissionId"] = new SelectList(_context.ApplicationPermissions, "Id", "Permission");
            return View();
        }

        // POST: Roles/CreateD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateD([Bind("RolId,ApplicationPermissionId")] DetalleRol detalleRol)
        {
            ModelState.Remove("Rol");
            ModelState.Remove("ApplicationPermission");
            if (ModelState.IsValid)
            {
                _context.Add(detalleRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Codigo", detalleRol.RolId);
            ViewData["ApplicationPermissionId"] = new SelectList(_context.ApplicationPermissions, "Id", "Permission", detalleRol.ApplicationPermissionId);
            return View(detalleRol);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Descripcion,Estado")] Role role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
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
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.Roles'  is null.");
            }
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> DeleteDR(int? id)
        {
            if (id == null || _context.DetalleRols == null)
            {
                return NotFound();
            }

            var detalleRole = await _context.DetalleRols
                .Include(a => a.ApplicationPermission)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleRole == null)
            {
                return NotFound();
            }

            return View(detalleRole);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("DeleteDR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedDR(int id)
        {
            if (_context.DetalleRols == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.DetalleRols'  is null.");
            }
            var detalleRole = await _context.DetalleRols.FindAsync(id);
            if (detalleRole != null)
            {
                _context.DetalleRols.Remove(detalleRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
          return (_context.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
