using CrudSecApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CrudSecApp.Controllers
{
    [Authorize(Roles = "1")]
    public class ApplicationsController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public ApplicationsController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index(string buscar, int pg=-1)
        {
            var pers = _context.Applications.Include(a => a.ApplicationModules);

            if (!String.IsNullOrEmpty(buscar))
            {
                pers = pers.Where(p => p.Name.Contains(buscar)).Include(a => a.ApplicationModules);
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

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Applications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,Code")] Application application)
        {
            DateTime now = DateTime.Now;

            application.TransaccionUid = Guid.NewGuid();
            application.FechaTransaccionUtc = now;
            application.FechaTransaccion = now;
            var s = @User.Identity!.Name;
            application.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            ModelState.Remove("RowVersion");

            if (ModelState.IsValid)
            {
                
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: Applications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,Code")] Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            application.TransaccionUid = Guid.NewGuid();
            application.FechaTransaccionUtc = DateTime.Now;
            application.FechaTransaccion = DateTime.Now;
            var s = @User.Identity!.Name;
            application.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.Id))
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
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Applications == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.Applications'  is null.");
            }
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
          return (_context.Applications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
