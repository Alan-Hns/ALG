using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudSecApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CrudSecApp.Controllers
{
    [Authorize(Roles = "1")]
    public class UserExtendedPermissionsController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public UserExtendedPermissionsController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        // GET: UserExtendedPermissions
        public async Task<IActionResult> Index()
        {
            var securityApplications_UATContext = _context.UserExtendedPermissions.Include(u => u.ApplicationUserPermission);
            return View(await securityApplications_UATContext.ToListAsync());
        }

        // GET: UserExtendedPermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserExtendedPermissions == null)
            {
                return NotFound();
            }

            var userExtendedPermission = await _context.UserExtendedPermissions
                .Include(u => u.ApplicationUserPermission)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userExtendedPermission == null)
            {
                return NotFound();
            }

            return View(userExtendedPermission);
        }

        // GET: UserExtendedPermissions/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserPermissionId"] = new SelectList(_context.ApplicationUserPermissions, "Id", "Id");
            return View();
        }

        // POST: UserExtendedPermissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserPermissionId,AcessCode,BusinessEntity,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion")] UserExtendedPermission userExtendedPermission)
        {
            userExtendedPermission.TransaccionUid = Guid.NewGuid();
            userExtendedPermission.FechaTransaccionUtc = DateTime.Now;
            userExtendedPermission.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            userExtendedPermission.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("RowVersion");
            ModelState.Remove("ApplicationUserPermission");

            if (ModelState.IsValid)
            {
                _context.Add(userExtendedPermission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserPermissionId"] = new SelectList(_context.ApplicationUserPermissions, "Id", "Id", userExtendedPermission.ApplicationUserPermissionId);
            return View(userExtendedPermission);
        }

        // GET: UserExtendedPermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserExtendedPermissions == null)
            {
                return NotFound();
            }

            var userExtendedPermission = await _context.UserExtendedPermissions.FindAsync(id);
            if (userExtendedPermission == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserPermissionId"] = new SelectList(_context.ApplicationUserPermissions, "Id", "Id", userExtendedPermission.ApplicationUserPermissionId);
            return View(userExtendedPermission);
        }

        // POST: UserExtendedPermissions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserPermissionId,AcessCode,BusinessEntity,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion")] UserExtendedPermission userExtendedPermission)
        {
            if (id != userExtendedPermission.Id)
            {
                return NotFound();
            }

            userExtendedPermission.TransaccionUid = Guid.NewGuid();
            userExtendedPermission.FechaTransaccionUtc = DateTime.Now;
            userExtendedPermission.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            userExtendedPermission.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("ApplicationUserPermission");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userExtendedPermission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExtendedPermissionExists(userExtendedPermission.Id))
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
            ViewData["ApplicationUserPermissionId"] = new SelectList(_context.ApplicationUserPermissions, "Id", "Id", userExtendedPermission.ApplicationUserPermissionId);
            return View(userExtendedPermission);
        }

        // GET: UserExtendedPermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserExtendedPermissions == null)
            {
                return NotFound();
            }

            var userExtendedPermission = await _context.UserExtendedPermissions
                .Include(u => u.ApplicationUserPermission)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userExtendedPermission == null)
            {
                return NotFound();
            }

            return View(userExtendedPermission);
        }

        // POST: UserExtendedPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserExtendedPermissions == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.UserExtendedPermissions'  is null.");
            }
            var userExtendedPermission = await _context.UserExtendedPermissions.FindAsync(id);
            if (userExtendedPermission != null)
            {
                _context.UserExtendedPermissions.Remove(userExtendedPermission);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExtendedPermissionExists(int id)
        {
          return (_context.UserExtendedPermissions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
