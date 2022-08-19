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
    public class ApplicationUserPermissionsController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public ApplicationUserPermissionsController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        // GET: ApplicationUserPermissions
        public async Task<IActionResult> Index(string buscar, int pg=1)
        {
            var pers = _context.ApplicationUserPermissions
                .Include(a => a.ApplicationPermission)
                .Include(a => a.ApplicationUser);

            if (!String.IsNullOrEmpty(buscar))
            {
                pers = pers.Where(p => p.ApplicationUser.Name.Contains(buscar) || p.ApplicationPermission.Permission.Contains(buscar)).Include(p => p.ApplicationPermission).Include(a => a.ApplicationUser);
            }

            const int pageSize = 8;
            if (pg < 1)
                pg = 1;

            int recsCount = pers.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = pers.Skip(recSkip).Take(pager.PageSize).ToListAsync();

            this.ViewBag.Pager = pager;

            return View(await data);
        }

        // GET: ApplicationUserPermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationUserPermissions == null)
            {
                return NotFound();
            }

            var applicationUserPermission = await _context.ApplicationUserPermissions
                .Include(a => a.ApplicationPermission)
                    .ThenInclude(a => a.ApplicationSection)
                        .ThenInclude(a => a.ApplicationModule)
                            .ThenInclude(a => a.Application)
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUserPermission == null)
            {
                return NotFound();
            }

            return View(applicationUserPermission);
        }

        // GET: ApplicationUserPermissions/Create
        public IActionResult Create()
        {
            ViewData["ApplicationPermissionId"] = new SelectList(_context.ApplicationPermissions, "Id", "Permission");
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Name");
            return View();
        }

        // POST: ApplicationUserPermissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,ApplicationPermissionId,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,ApplicationId,Site")] ApplicationUserPermission applicationUserPermission)
        {
            applicationUserPermission.TransaccionUid = Guid.NewGuid();
            applicationUserPermission.FechaTransaccionUtc = DateTime.Now;
            applicationUserPermission.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationUserPermission.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("RowVersion");
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("ApplicationPermission");

            if (ModelState.IsValid)
            {
                _context.Add(applicationUserPermission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationPermissionId"] = new SelectList(_context.ApplicationPermissions, "Id", "Permission", applicationUserPermission.ApplicationPermissionId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Name", applicationUserPermission.ApplicationUserId);
            return View(applicationUserPermission);
        }

        // GET: ApplicationUserPermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationUserPermissions == null)
            {
                return NotFound();
            }

            var applicationUserPermission = await _context.ApplicationUserPermissions.FindAsync(id);
            if (applicationUserPermission == null)
            {
                return NotFound();
            }
            ViewData["ApplicationPermissionId"] = new SelectList(_context.ApplicationPermissions, "Id", "Permission", applicationUserPermission.ApplicationPermissionId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Name", applicationUserPermission.ApplicationUserId);
            return View(applicationUserPermission);
        }

        // POST: ApplicationUserPermissions/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId,ApplicationPermissionId,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,ApplicationId,Site")] ApplicationUserPermission applicationUserPermission)
        {
            if (id != applicationUserPermission.Id)
            {
                return NotFound();
            }

            applicationUserPermission.TransaccionUid = Guid.NewGuid();
            applicationUserPermission.FechaTransaccionUtc = DateTime.Now;
            applicationUserPermission.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationUserPermission.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            //ModelState.Remove("ApplicationUserPermission");
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("ApplicationPermission");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUserPermission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserPermissionExists(applicationUserPermission.Id))
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
            ViewData["ApplicationPermissionId"] = new SelectList(_context.ApplicationPermissions, "Id", "Permission", applicationUserPermission.ApplicationPermissionId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Name", applicationUserPermission.ApplicationUserId);
            return View(applicationUserPermission);
        }

        // GET: ApplicationUserPermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicationUserPermissions == null)
            {
                return NotFound();
            }

            var applicationUserPermission = await _context.ApplicationUserPermissions
                .Include(a => a.ApplicationPermission)
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUserPermission == null)
            {
                return NotFound();
            }

            return View(applicationUserPermission);
        }

        // POST: ApplicationUserPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicationUserPermissions == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.ApplicationUserPermissions'  is null.");
            }
            var applicationUserPermission = await _context.ApplicationUserPermissions.FindAsync(id);
            if (applicationUserPermission != null)
            {
                _context.ApplicationUserPermissions.Remove(applicationUserPermission);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserPermissionExists(int id)
        {
          return (_context.ApplicationUserPermissions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
