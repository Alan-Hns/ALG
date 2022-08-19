using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudSecApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CrudSecApp.Models.UsViewModels;

namespace CrudSecApp.Controllers
{
    [Authorize(Roles = "1")]
    public class ApplicationUsersController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public ApplicationUsersController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index(string buscar, int pg = 1)
        {

            var pers = _context.ApplicationUsers.Include(a => a.ApplicationUserRole);

            if (!String.IsNullOrEmpty(buscar))
            {
                pers = pers.Where(u => u.UserName!.Contains(buscar) || u.Name!.Contains(buscar)).Include(u => u.ApplicationUserRole);
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

        //: ApplicationUserRol
        public async Task<ActionResult> IndexUR(int? id) //Importanterevisar este resultado
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var viewModel = new ModelUR
            {
                Users = await _context.ApplicationUsers
                .Include(a => a.ApplicationUserPermissions)
                    .ThenInclude(a => a.ApplicationPermission)
                .Where(a => a.Id == id)
                .ToListAsync(),
                AppUserRols = await _context.ApplicationUserRols
                .Include(a => a.Roles)
                    .ThenInclude(a => a.DetalleRols)
                        .ThenInclude(a => a.ApplicationPermission)
                .Include(a => a.ApplicationUser)
                    .ThenInclude(a => a.ApplicationUserPermissions)
                        .ThenInclude(a => a.ApplicationPermission)
                .Where(a => a.ApplicationUser.Id == id)
                .AsNoTracking()
                .ToListAsync()
            };

            return View(viewModel);
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers
                .Include(a => a.ApplicationUserRole)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserRoleId"] = new SelectList(_context.ApplicationUserRoles, "Id", "Roles");
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string username, [Bind("Id,Name,UserName,Password,Email,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,IsActive,LastDateStartSession,ApplicationUserRoleId")] ApplicationUser applicationUser)
        {
            var users = (_context.ApplicationUsers?.Any(e => e.UserName == username)).GetValueOrDefault();

            if (users == false)
            {
                applicationUser.TransaccionUid = Guid.NewGuid();
                applicationUser.FechaTransaccionUtc = DateTime.Now;
                applicationUser.FechaTransaccion = DateTime.Now;
                applicationUser.LastDateStartSession = DateTime.Now;
                var s = @User.Identity!.Name;
                applicationUser.ModificadoPor = s!;

                ModelState.Remove("ModificadoPor");
                ModelState.Remove("RowVersion");

                if (ModelState.IsValid)
                {
                    _context.Add(applicationUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ApplicationUserRoleId"] = new SelectList(_context.ApplicationUserRoles, "Id", "Roles", applicationUser.ApplicationUserRoleId);
                return View(applicationUser);
            }
            else
            {
                Console.WriteLine("Usuario ya existe");
                return Json(new { StatusCode = false, message = "Usuario ya existe" });
            }

        }

        // GET: ApplicationUserRol/createUR
        public IActionResult CreateUR()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Name");
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Codigo");
            return View();
        }

        // POST: ApplicationUserRol/CreateUR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUR([Bind("ApplicationUserId,RolesId")] ApplicationUserRol applicationUserRol)
        {
            ModelState.Remove("Roles");
            ModelState.Remove("ApplicationUser");
            if (ModelState.IsValid)
            {
                _context.Add(applicationUserRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Name", applicationUserRol.ApplicationUserId);
            ViewData["RolesId"] = new SelectList(_context.Roles, "Id", "Codigo", applicationUserRol.RolesId);
            return View(applicationUserRol);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers
                .FindAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserRoleId"] = new SelectList(_context.ApplicationUserRoles, "Id", "Roles", applicationUser.ApplicationUserRoleId);
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserName,Password,Email,TransaccionUid,TipoTransaccion,FechaTransaccionUtc,DescripcionTransaccion,FechaTransaccion,ModificadoPor,RowVersion,IsActive,LastDateStartSession,ApplicationUserRoleId")] ApplicationUser applicationUser)
        {

            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            applicationUser.TransaccionUid = Guid.NewGuid();
            applicationUser.FechaTransaccionUtc = DateTime.Now;
            applicationUser.FechaTransaccion = DateTime.Now;
            applicationUser.LastDateStartSession = DateTime.Now;
            var s = @User.Identity!.Name;
            applicationUser.ModificadoPor = s!;

            ModelState.Remove("ModificadoPor");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["ApplicationUserRoleId"] = new SelectList(_context.ApplicationUserRoles, "Id", "Roles", applicationUser.ApplicationUserRoleId);
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUsers
                .Include(a => a.ApplicationUserRole)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicationUsers == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.ApplicationUsers'  is null.");
            }
            var applicationUser = await _context.ApplicationUsers.FindAsync(id);
            if (applicationUser != null)
            {
                _context.ApplicationUsers.Remove(applicationUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> DeleteR(int? id)
        {
            if (id == null || _context.ApplicationUserRols == null)
            {
                return NotFound();
            }

            var applicationUserRol = await _context.ApplicationUserRols
                .Include(a => a.Roles)
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUserRol == null)
            {
                return NotFound();
            }

            return View(applicationUserRol);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("DeleteR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedR(int id)
        {
            if (_context.ApplicationUserRols == null)
            {
                return Problem("Entity set 'SecurityApplications_UATContext.ApplicationRols'  is null.");
            }
            var applicationUserRol = await _context.ApplicationUserRols.FindAsync(id);
            if (applicationUserRol != null)
            {
                _context.ApplicationUserRols.Remove(applicationUserRol);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(int id)
        {
            return (_context.ApplicationUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
