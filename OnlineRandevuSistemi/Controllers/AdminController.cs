using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRandevuSistemi.DataAccess;
using OnlineRandevuSistemi.Entites.UserToken;
using OnlineRandevuSistemi.Services;
using OnlineRandevuSistemi.Services.Repo;
using Org.BouncyCastle.Crypto.Generators;

namespace OnlineRandevuSistemi.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentService _appointmentRepository;

        public AdminController(IUserRepository userRepository, IAppointmentService appointmentRepository)
        {
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
        }


        public async Task<IActionResult> Users()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı zaten alınmış.");
                return View(user);
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash); 

            await _userRepository.AddUserAsync(user);
            return RedirectToAction("Users");
        }


        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            return RedirectToAction("Users");
        }


      
        public IActionResult Appointments()
        {
            var appointments =  _appointmentRepository.GetAllAppointmentsAsync();
            return View(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, string status)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();

            appointment.Status = status;
            await _appointmentRepository.UpdateAppointmentAsync(appointment);
            return RedirectToAction("Appointments");
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(int userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            user.Role = role;
            await _userRepository.UpdateUserAsync(user);
            return Json(new { success = true });
        }
    }
}
