using Hachodromo.Web.Data;

namespace Hachodromo.Web.Services
{
    public class DeletedUserService
    {
        private readonly ApplicationDbContext _context;

        public DeletedUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para actualizar el PasswordHash de un usuario
        public void UpdatePasswordHash(string email, string newPassword)
        {
            // Buscar al usuario por su email
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                // Generar el hash de la nueva contraseña
                string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

                // Actualizar el campo PasswordHash
                user.PasswordHash = newPasswordHash;

                // Guardar los cambios en la base de datos
                _context.SaveChanges();
            }
        }
    }
}
