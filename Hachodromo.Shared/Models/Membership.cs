using System.ComponentModel.DataAnnotations;

namespace Hachodromo.Shared.Models
{
	public class Membership
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string MembershipType { get; set; } = string.Empty; // Ejemplo: Gold, Silver, Bronze

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime ExpirationDate { get; set; }

		// Relación con User
		public int UserId { get; set; }
		public User User { get; set; } = null!;

		// Validación: Asegurarse de que StartDate < ExpirationDate
		public bool IsValidDateRange() => StartDate < ExpirationDate;

	}

}
