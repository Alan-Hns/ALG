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
    public class ApplicationPermissionsController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public ApplicationPermissionsController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        // GET: ApplicationPermissions
        public async Task<IActionResult> Index(string buscar, int pg=1)
        {

            var pers = _context.ApplicationPermissions.Include(a => a.ApplicationSection);

            if (!String.IsNullOrEmpty(buscar))
            {
                pers = pers.Where(p => p.Permission.Contains(buscar) || p.ApplicationSection.Section.Contains(buscar)).Include(p => p.ApplicationSection);
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

        // GET: ApplicationPermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationPermissions == null)
            {
                return NotFound();
            }

            var applicationPermission = await _context.ApplicationPermissions
                .Include(a => a.ApplicationSection)
                    .ThenInclude(b => b.ApplicationModule)
                        .ThenInclude(c => c.Application)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationPermission == null)
            {
                return NotFound();
            }

            return View(applicationPermission);
        }

        // GET: ApplicationPermissions/Create
        public IActionResult Create()
        {
            ViewData["ApplicationSectionId"] = new SelectList(_context.ApplicationSections, "Id", "Section");
            return View();
        }

        // POST: ApplicationPermissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Permission,IsActive,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,Group,Code,ApplicationSectionId")] ApplicationPermission applicationPermission)
        {
            applicationPermission.TransaccionUid = Guid.NewGuid();
            applicationPermission.FechaTransaccionUtc = DateTime.Now;
            applicationPermission.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationPermission.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("RowVersion");
            ModelState.Remove("ApplicationSection");
            ModelState.Remove("DetalleRols");

            if (ModelState.IsValid)
            {
                _context.Add(applicationPermission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationSectionId"] = new SelectList(_context.ApplicationSections, "Id", "Section", applicationPermission.ApplicationSectionId);
            return View(applicationPermission);
        }

        // GET: ApplicationPermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationPermissions == null)
            {
                return NotFound();
            }

            var applicationPermission = await _context.ApplicationPermissions.FindAsync(id);
            if (applicationPermission == null)
            {
                return NotFound();
            }
            ViewData["ApplicationSectionId"] = new SelectList(_context.ApplicationSections, "Id", "Section", applicationPermission.ApplicationSectionId);
            return View(applicationPermission);
        }

        // POST: ApplicationPermissions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Permission,IsActive,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,Group,Code,ApplicationSectionId")] ApplicationPermission applicationPermission)
        {
            if (id != applicationPermission.Id)
            {
                return NotFound();
            }

            applicationPermission.TransaccionUid = Guid.NewGuid();
            applicationPermission.FechaTransaccionUtc = DateTime.Now;
            applicationPermission.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationPermission.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("ApplicationSection");
            ModelState.Remove("DetalleRols");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationPermission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationPermissionExists(applicationPermission.Id))
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
            ViewData["ApplicationSectionId"] = new SelectList(_context.ApplicationSections, "Id", "Section", applicationPermission.ApplicationSectionId);
            return View(applicationPermission);
        }

        // GET: ApplicationPermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicationPermissions == null)
            {
                return NotFound();
            }

            var applicationPermission = await _context.ApplicationPermissions
                .Include(a => a.ApplicationSection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationPermission == null)
            {
                return NotFound();
            }

            return View(applicationPermission);
        }

        // POST: ApplicationPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicationPermissions == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.ApplicationPermissions'  is null.");
            }
            var applicationPermission = await _context.ApplicationPermissions.FindAsync(id);
            if (applicationPermission != null)
            {
                _context.ApplicationPermissions.Remove(applicationPermission);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationPermissionExists(int id)
        {
          return (_context.ApplicationPermissions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
