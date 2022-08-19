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
    public class ApplicationSectionsController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public ApplicationSectionsController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        //GET: ApplicationSections

        public async Task<IActionResult> Index(string buscar, int pg=1)
        {
            var pers = _context.ApplicationSections.Include(a => a.ApplicationModule);

            if (!String.IsNullOrEmpty(buscar))
            {
                pers = pers.Where(p => p.Section.Contains(buscar) || p.ApplicationModule.Module.Contains(buscar)).Include(p => p.ApplicationModule);
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

        // GET: ApplicationSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationSections == null)
            {
                return NotFound();
            }

            var applicationSection = await _context.ApplicationSections
                .Include(a => a.ApplicationModule)
                    .ThenInclude(a => a.Application)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationSection == null)
            {
                return NotFound();
            }

            return View(applicationSection);
        }

        // GET: ApplicationSections/Create
        public IActionResult Create()
        {
            ViewData["ApplicationModuleId"] = new SelectList(_context.ApplicationModules, "Id", "Module");
            return View();
        }

        // POST: ApplicationSections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Section,ApplicationModuleId,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,IconName")] ApplicationSection applicationSection)
        {
            applicationSection.TransaccionUid = Guid.NewGuid();
            applicationSection.FechaTransaccionUtc = DateTime.Now;
            applicationSection.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationSection.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("RowVersion");
            ModelState.Remove("ApplicationModule");

            if (ModelState.IsValid)
            {
                _context.Add(applicationSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationModuleId"] = new SelectList(_context.ApplicationModules, "Id", "Module", applicationSection.ApplicationModuleId);
            return View(applicationSection);
        }

        // GET: ApplicationSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationSections == null)
            {
                return NotFound();
            }

            var applicationSection = await _context.ApplicationSections.FindAsync(id);
            if (applicationSection == null)
            {
                return NotFound();
            }
            ViewData["ApplicationModuleId"] = new SelectList(_context.ApplicationModules, "Id", "Module", applicationSection.ApplicationModuleId);
            return View(applicationSection);
        }

        // POST: ApplicationSections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Section,ApplicationModuleId,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,IconName")] ApplicationSection applicationSection)
        {
            if (id != applicationSection.Id)
            {
                return NotFound();
            }

            applicationSection.TransaccionUid = Guid.NewGuid();
            applicationSection.FechaTransaccionUtc = DateTime.Now;
            applicationSection.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationSection.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("ApplicationModule");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationSectionExists(applicationSection.Id))
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
            ViewData["ApplicationModuleId"] = new SelectList(_context.ApplicationModules, "Id", "Module", applicationSection.ApplicationModuleId);
            return View(applicationSection);
        }

        // GET: ApplicationSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicationSections == null)
            {
                return NotFound();
            }

            var applicationSection = await _context.ApplicationSections
                .Include(a => a.ApplicationModule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationSection == null)
            {
                return NotFound();
            }

            return View(applicationSection);
        }

        // POST: ApplicationSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicationSections == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.ApplicationSections'  is null.");
            }
            var applicationSection = await _context.ApplicationSections.FindAsync(id);
            if (applicationSection != null)
            {
                _context.ApplicationSections.Remove(applicationSection);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationSectionExists(int id)
        {
          return (_context.ApplicationSections?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
