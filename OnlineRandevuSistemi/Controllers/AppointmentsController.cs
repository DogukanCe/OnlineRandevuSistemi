using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRandevuSistemi.DataAccess;
using OnlineRandevuSistemi.Entites;
using OnlineRandevuSistemi.Services;
using OnlineRandevuSistemi.Services.Repo;
using System.Security.Claims;

namespace OnlineRandevuSistemi.Controllers
{
    [Authorize(Roles = "User")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceRepository _serviceRepository;
        public AppointmentsController(IAppointmentService appointmentService, IServiceRepository serviceRepository)
        {
            _appointmentService = appointmentService;
            _serviceRepository = serviceRepository;

        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int userId))
            {
                Console.WriteLine($"User ID: {userId}");
            }
            else
            {
                Console.WriteLine("User ID is null or invalid!");
            }

            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId);
            var services =  _serviceRepository.GetAllServicesAsync();

            ViewBag.Services = services; 
            return View(appointments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        public IActionResult Create()
        {
            ViewBag.Services =  _appointmentService.GetAllAppointmentsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.AddAppointmentAsync(appointment);
                return Json(new { success = true, message = "Randevu başarıyla eklendi!" });
            }
            return Json(new { success = false, message = "Geçersiz veri girişi!" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            var existingAppointment = await _appointmentService.GetAppointmentByIdAsync(appointment.Id);
            if (existingAppointment == null) return NotFound();

            existingAppointment.AppointmentDate = appointment.AppointmentDate;
            existingAppointment.ServiceId = appointment.ServiceId;

            await _appointmentService.UpdateAppointmentAsync(existingAppointment);
            return Json(new { success = true });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);
            return Json(new { success = true });
        }
    }
}
