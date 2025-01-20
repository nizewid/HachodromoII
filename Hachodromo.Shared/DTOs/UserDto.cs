using Hachodromo.Shared.Constants;

namespace Hachodromo.Shared.DTOs
{
	public class UserDto
	{
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string LastName2 { get; set; }
        public SexCode Sex { get; set; } // Cambiado de int a SexCode
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public Region Region { get; set; }
        public bool IsActive { get; set; }
        public int UserType { get; set; } = 3 ; // Por defecto Cliente Básico
        public ICollection<MembershipDto>? Memberships { get; set; }
        public DateTime BornDate { get; set; }
    }


}
