using OnlineRandevuSistemi.DataAccess;
using OnlineRandevuSistemi.DataAccess.Db;
using OnlineRandevuSistemi.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRandevuSistemi.Services
{
    public class ServiceServices : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Service> GetAllServicesAsync()
        {
            return  _context.Services.ToList();
        }

        public async Task<Service> GetServiceByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task AddServiceAsync(Service service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateServiceAsync(Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}
