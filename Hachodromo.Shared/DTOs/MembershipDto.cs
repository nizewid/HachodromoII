namespace Hachodromo.Shared.DTOs
{
	public class MembershipDto
	{
		public string MembershipType { get; set; } = null!;// Ejemplo: Gold, Silver, Bronze
		public DateTime StartDate { get; set; } // Fecha de inicio de la membresía
		public DateTime ExpirationDate { get; set; } // Fecha de vencimiento
	}
}
