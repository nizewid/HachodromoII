using Hachodromo.Shared.Constants;
using Hachodromo.Shared.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hachodromo.Shared.Models
{
	public class User
	{
		[Key]  // Clave primaria
		public int Id { get; set; }

		// Información Personal
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; } = string.Empty;

		[MaxLength(50)] // Opcional Segundo apellido
		public string LastName2 { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Date)] // Fecha de nacimiento
		public DateTime BornDate { get; set; }

		[Required]
		public SexCode Sex { get; set; } // Usamos el enum SexCode

		[Required]
		[MaxLength(50)] // Ciudad de residencia
		public string City { get; set; } = string.Empty;

		[Required]
		public Region Region { get; set; } // Comunidad Autónoma (Region)

		// Autenticación
		[Required]
		[EmailAddress] // Validación de Email para Login
		public string Email { get; set; } = string.Empty;

		[Required]
		[MinLength(8)] // Longitud mínima para el hash de la contraseña
		public string PasswordHash { get; set; } = string.Empty;

		[Required]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación

		public DateTime? LastLogin { get; set; } // Último acceso (nullable)

		public bool IsActive { get; set; } = true; // Estado de activación de la cuenta

		// Relaciones
		[Required]
		[ForeignKey(nameof(UserType))] // Clave foránea para UserType
		public int UserTypeId { get; set; }

		public UserType UserType { get; set; } = null!; // Relación con UserType

		[Required]
		public ICollection<Membership> Memberships { get; set; } = new List<Membership>();// Relación con Membresía (Gold, Silver, Bronze)

		// Fecha de vencimiento de la membresía
	}
}
