using Microsoft.AspNetCore.Mvc;
using CrudSecApp.Models;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace CrudSecApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly SecurityApplications_UATContext _context;

        public LoginController(SecurityApplications_UATContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult Login()
        {
            return View();
        }

        // POST
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string password, ApplicationUser applicationUser)
        {
            var user = _context.ApplicationUsers.Where(x => x.UserName == applicationUser.UserName && x.Password == applicationUser.Password).FirstOrDefault();
            var users = (_context.ApplicationUsers?.Any(e => e.Password == password)).GetValueOrDefault();


            if (user != null)
            {
                var claims = new List<Claim>()
                                        {
                                            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                                            new Claim(ClaimTypes.Name, user.Name),
                                            new Claim(ClaimTypes.Role, Convert.ToString(user.ApplicationUserRoleId)!),
                                            new Claim("Programer", "Alan :)")
                                        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    //IsPersistent = applicationUser.RememberLogin
                });
                ViewBag.Message = "Bienvenido";
                Console.WriteLine("Bienvenido");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Console.WriteLine("Usuario Incorrecto");
                return Json(new { StatusCode = false, message = "Datos Incorrectos" });
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
