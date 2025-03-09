using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRandevuSistemi.Entites;
using OnlineRandevuSistemi.Services;
using System.Security.Claims;

namespace OnlineRandevuSistemi.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IService _serviceService;

        public UserController(IAppointmentService appointmentService, IService serviceService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId);
            return View(appointments);
        }

    

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Appointment appointment)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            appointment.UserId = userId;
            appointment.Status = "Beklemede";

            await _appointmentService.AddAppointmentAsync(appointment);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update(Appointment appointment)
        {
            await _appointmentService.UpdateAppointmentAsync(appointment);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);
            return Ok();
        }
       
        public async Task<IActionResult> GetAppointments()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId);
            return PartialView("_AppointmentList", appointments);
        }


    }
}
