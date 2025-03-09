using Microsoft.EntityFrameworkCore;
using OnlineRandevuSistemi.Entites;
using OnlineRandevuSistemi.Services.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRandevuSistemi.Services
{
    public class AppointmentServices : IAppointmentService
    {
        private readonly IAppointmentService _appointmentRepository;

        public AppointmentServices(IAppointmentService appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAppointmentsAsync();
        }


        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(id);
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepository.AddAppointmentAsync(appointment);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            await _appointmentRepository.DeleteAppointmentAsync(id);
        }

        Task<IEnumerable<Appointment>> IAppointmentService.GetAllAppointmentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(int userId)
        {
            return await _appointmentRepository.GetAppointmentsByUserIdAsync(userId);
        }

    }
}
