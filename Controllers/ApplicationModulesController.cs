using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudSecApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CrudSecApp.Controllers
{
    [Authorize(Roles = "1")]
    public class ApplicationModulesController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public ApplicationModulesController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        // GET: ApplicationModules
        public async Task<IActionResult> Index(string buscar,string appGenre, int pg=1)
        {

            var pers = _context.ApplicationModules.Include(a => a.Application);

            if (!String.IsNullOrEmpty(buscar))
            {
                pers = pers.Where(p => p.Module.Contains(buscar) || p.Application.Name.Contains(buscar)).Include(p => p.Application);
            }
            if (!string.IsNullOrEmpty(appGenre))
            {
                pers = pers.Where(x => x.Application.Name == appGenre).Include(a => a.Application);
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

        // GET: ApplicationModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationModules == null)
            {
                return NotFound();
            }

            var applicationModule = await _context.ApplicationModules
                .Include(a => a.Application)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationModule == null)
            {
                return NotFound();
            }

            return View(applicationModule);
        }

        // GET: ApplicationModules/Create
        public IActionResult Create()
        {
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name");
            return View();
        }

        // POST: ApplicationModules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Module,ApplicationId,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion")] ApplicationModule applicationModule)
        {
            applicationModule.TransaccionUid = Guid.NewGuid();
            applicationModule.FechaTransaccionUtc = DateTime.Now;
            applicationModule.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationModule.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("RowVersion");
            ModelState.Remove("Application");

            if (ModelState.IsValid)
            {
                _context.Add(applicationModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name", applicationModule.ApplicationId);
            return View(applicationModule);
        }

        // GET: ApplicationModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationModules == null)
            {
                return NotFound();
            }

            var applicationModule = await _context.ApplicationModules.FindAsync(id);
            if (applicationModule == null)
            {
                return NotFound();
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name", applicationModule.ApplicationId);
            return View(applicationModule);
        }

        // POST: ApplicationModules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Module,ApplicationId,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion")] ApplicationModule applicationModule)
        {
            if (id != applicationModule.Id)
            {
                return NotFound();
            }

            applicationModule.TransaccionUid = Guid.NewGuid();
            applicationModule.FechaTransaccionUtc = DateTime.Now;
            applicationModule.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationModule.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("Application");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationModuleExists(applicationModule.Id))
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
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name", applicationModule.ApplicationId);
            return View(applicationModule);
        }

        // GET: ApplicationModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicationModules == null)
            {
                return NotFound();
            }

            var applicationModule = await _context.ApplicationModules
                .Include(a => a.Application)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationModule == null)
            {
                return NotFound();
            }

            return View(applicationModule);
        }

        // POST: ApplicationModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicationModules == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.ApplicationModules'  is null.");
            }
            var applicationModule = await _context.ApplicationModules.FindAsync(id);
            if (applicationModule != null)
            {
                _context.ApplicationModules.Remove(applicationModule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationModuleExists(int id)
        {
          return (_context.ApplicationModules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
