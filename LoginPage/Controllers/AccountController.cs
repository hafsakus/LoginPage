using Microsoft.AspNetCore.Mvc;
using LoginPage.Data;
using LoginPage.Models;

namespace LoginPage.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Veritabanı bağlantısını Constructor ile içeri alıyoruz (Dependency Injection)
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // SQL Sorgusu: Hem Username hem Email ile kontrol et!
            var user = _context.Users.FirstOrDefault(u =>
                (u.UserName == model.UsernameOrEmail || u.Email == model.UsernameOrEmail)
                && u.Password == model.Password);

            if (user != null)
            {
                // Giriş başarılı!
                return RedirectToAction("Index", "Assignments");
            }

            // Kullanıcı bulunamadıysa uyarı ver
            ModelState.AddModelError("", "Kullanıcı adı/E-posta veya şifre hatalı!");
            return View(model);
        }
    }
}
