using Microsoft.AspNetCore.Mvc;
using OnlineRandevuSistemi.Entites;
using OnlineRandevuSistemi.Services;

namespace OnlineRandevuSistemi.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IService _serviceService;

        public ServiceController(IService serviceService)
        {
            _serviceService = serviceService;
        }

    
        public IActionResult Index()
        {
            var services =  _serviceService.GetAllServicesAsync();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                await _serviceService.AddServiceAsync(service);
                return RedirectToAction("Index");
            }
            return View(service);
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                await _serviceService.UpdateServiceAsync(service);
                return RedirectToAction("Index");
            }
            return View(service);
        }

  
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceService.DeleteServiceAsync(id);
            return RedirectToAction("Index");
        }
    }
}
