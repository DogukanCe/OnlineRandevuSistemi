using Microsoft.EntityFrameworkCore;
using OnlineRandevuSistemi.Entites;
using OnlineRandevuSistemi.Entites.UserToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRandevuSistemi.DataAccess.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Service>()
        .Property(s => s.Price)
        .HasPrecision(18, 2); // Toplam 18 basamak, 2 ondalık basamak

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-BA0TPT1\\LOCALHOST;Database=OnlineRandevuSistemi;User Id=sa;Password=1234;Integrated Security=True;TrustServerCertificate=Yes",
                    b => b.MigrationsAssembly("OnlineRandevuSistemi.DataAccess"));
            }
        }



    }
}
