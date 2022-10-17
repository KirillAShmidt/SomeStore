using Microsoft.AspNetCore.Mvc;
using SomeStore.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace SomeStore.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext _db;

        public HomeController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Products);
        }

        public IActionResult Product(int id)
        {
            var product = _db.Products?
                .FirstOrDefault(p => p.Id == id);

            return View(product);
        }

        public IActionResult Shop()
        {
            return View(_db.Products);
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> UserLogin()
        {
            var form = HttpContext.Request.Form;

            if (!form.ContainsKey("login") || !form.ContainsKey("password"))
                return View(Results.BadRequest());

            string login = form["login"];
            string password = form["password"];

            User? user = _db.Users.FirstOrDefault(p => p.Login == login && p.Password == password);

            if (user is null)
                return Redirect("/login");

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login)};
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Redirect("/");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/");
        }

        public IActionResult SubmitUser()
        {
            var form = HttpContext.Request.Form;

            if (!form.ContainsKey("login") || !form.ContainsKey("password"))
                return View(Results.BadRequest());

            string login = form["login"];
            string password = form["password"];

            _db.Users.Add(new User { Login = login, Password = password});
            _db.SaveChanges();

            return Redirect("/Login");
        }

        public IActionResult AddNewUser()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}