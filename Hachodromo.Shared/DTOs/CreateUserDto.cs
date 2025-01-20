using Hachodromo.Shared.Constants;

namespace Hachodromo.Shared.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; } // Agregado
        public DateTime BornDate { get; set; } // Agregado
        public string Email { get; set; }
        public SexCode Sex { get; set; } // Cambiado de int a SexCode
        public string City { get; set; }
        public Region Region { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }  // Se incluye para el registro
        public int UserType { get; set; }  // Se puede mantener como nombre o ID
        public List<MembershipDto> Memberships { get; set; }
    }
}
