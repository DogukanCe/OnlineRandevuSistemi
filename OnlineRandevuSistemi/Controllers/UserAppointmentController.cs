using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRandevuSistemi.DataAccess;
using OnlineRandevuSistemi.Entites;
using OnlineRandevuSistemi.Services;
using System.Security.Claims;

namespace OnlineRandevuSistemi.Controllers
{
    [Authorize(Roles = "User")]
    public class UserAppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceRepository _serviceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAppointmentController(IAppointmentService appointmentService, IServiceRepository serviceRepository, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _serviceRepository = serviceRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId);
            return View(appointments);
        }

     
        public IActionResult Create()
        {
            ViewBag.Services =  _serviceRepository.GetAllServicesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            appointment.UserId = userId;
            appointment.Status = "Pending"; 

            await _appointmentService.AddAppointmentAsync(appointment);
            return RedirectToAction("Index");
        }

      
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null || appointment.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Unauthorized();
            }

            ViewBag.Services =  _serviceRepository.GetAllServicesAsync();
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (appointment.UserId != userId)
            {
                return Unauthorized();
            }

            await _appointmentService.UpdateAppointmentAsync(appointment);
            return RedirectToAction("Index");
        }

      
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null || appointment.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Unauthorized();
            }

            appointment.Status = "Cancelled";
            await _appointmentService.UpdateAppointmentAsync(appointment);
            return RedirectToAction("Index");
        }
    }
}
