using Hachodromo.Shared.Constants;
using Hachodromo.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Hachodromo.Web.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<User> Users { get; set; } = null!;
		public DbSet<UserType> UserTypes { get; set; } = null!;
		public DbSet<Membership> Memberships { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configuraciones adicionales del modelo
			modelBuilder.Entity<User>()
				.HasMany(u => u.Memberships)
				.WithOne(m => m.User)
				.HasForeignKey(m => m.UserId);

			modelBuilder.Entity<UserType>()
				.HasMany(ut => ut.Users)
				.WithOne(u => u.UserType)
				.HasForeignKey(u => u.UserTypeId);
		}

        public void Seed()
        {
            // Verifica si ya existen datos en las tablas, si no, agrega algunos datos predeterminados
            if (!UserTypes.Any())
            {
                var userType = new UserType
                {
                    Name = "SuperAdmin",
                    Description = "Administrador del sistema"
                };
                UserTypes.Add(userType);
                SaveChanges();
            }

            if (!Users.Any())
            {
                var userType = UserTypes.FirstOrDefault(u => u.Name == "SuperAdmin");

                if (userType != null)
                {
                    var user = new User
                    {
                        FirstName = "Jose Gregorio",
                        LastName = "Flores",
                        LastName2 = "Silva",
                        BornDate = new DateTime(1989, 9, 19),
                        Sex = SexCode.Male,
                        City = "Gijon",
                        Region = Region.Asturias,
                        Email = "admin@admin.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"), // Hash de la contraseña
                        CreatedAt = DateTime.Now,
                        LastLogin = DateTime.Now,
                        IsActive = true,
                        UserTypeId = userType.Id,
                    };
                    Users.Add(user);
                    SaveChanges();
                }
            }
        }
    }
}
