namespace Hachodromo.Shared.DTOs
{
	public class UserDto
	{
		public int Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public int Region { get; set; }
		public bool IsActive { get; set; }
		public string UserType { get; set; } = string.Empty; // Simplificado, en vez de incluir el objeto completo de UserType
		public ICollection<MembershipDto>? Memberships { get; set; } // Relación con MembershipDto
	}


}
