using Microsoft.AspNetCore.Mvc;
using OnlineRandevuSistemi.DataAccess;
using OnlineRandevuSistemi.Models;
using OnlineRandevuSistemi.Services;
using Org.BouncyCastle.Crypto.Generators;

namespace OnlineRandevuSistemi.Controllers.Auth
{
  
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || user.PasswordHash != password)
            {
                ViewBag.Error = "Geçersiz kullanıcı adı veya şifre!";
                return View();
            }



            _httpContextAccessor.HttpContext.Session.SetString("UserRole", user.Role);
            _httpContextAccessor.HttpContext.Session.SetInt32("UserId", user.Id);

            if (user.Role == "Admin")
           {
                return RedirectToAction("Users", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Appointments");
            }


        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
