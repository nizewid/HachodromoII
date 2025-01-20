using Hachodromo.Shared.Constants;
using Hachodromo.Shared.DTOs;
using Hachodromo.Shared.Interfaces;
using Hachodromo.Shared.Models;
using Hachodromo.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hachodromo.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public UsersController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Sex = user.Sex, // Cambiado de int a SexCode
                    Email = user.Email,
                    City = user.City,
                    Region = (Region)(int)user.Region,
                    IsActive = user.IsActive,
                    UserType = user.UserType.Name,
                    Memberships = user.Memberships.Select(m => new MembershipDto
                    {
                        MembershipType = m.MembershipType,
                        StartDate = m.StartDate,
                        ExpirationDate = m.ExpirationDate
                    }).ToList()
                }).ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Sex = user.Sex, // Cambiado de int a SexCode
                    Email = user.Email,
                    City = user.City,
                    Region = (Region)(int)user.Region,
                    IsActive = user.IsActive,
                    UserType = user.UserType.Name,
                    Memberships = user.Memberships.Select(m => new MembershipDto
                    {
                        MembershipType = m.MembershipType,
                        StartDate = m.StartDate,
                        ExpirationDate = m.ExpirationDate
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                // Asegúrate de que el DTO contiene los datos requeridos
                if (createUserDto == null)
                {
                    return BadRequest("Los datos proporcionados son inválidos.");
                }

                // Hash de la contraseña antes de guardarla
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

                // Crear el objeto User a partir de CreateUserDto
                var user = new User
                {
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    LastName2 = createUserDto.LastName2,
                    BornDate = createUserDto.BornDate,
                    Sex = createUserDto.Sex, // Cambiado de int a SexCode
                    Email = createUserDto.Email,
                    City = createUserDto.City,
                    Region = (Region)createUserDto.Region,
                    IsActive = createUserDto.IsActive,
                    PasswordHash = passwordHash,  // Usar el hash de la contraseña
                    UserTypeId = createUserDto.UserType, // Asignar el UserTypeId directamente
                    Memberships = createUserDto.Memberships?.Select(m => new Membership
                    {
                        MembershipType = m.MembershipType,
                        StartDate = m.StartDate,
                        ExpirationDate = m.ExpirationDate
                    }).ToList()
                };

                // Imprimir los detalles del objeto User antes de guardarlo
                PrintUserDetails(user);

                // Guardar el objeto User en la base de datos
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Crear el objeto UserDto a partir del objeto User recién creado
                var userDto = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    LastName2 = user.LastName2,
                    BornDate = user.BornDate,
                    Sex = user.Sex, // Cambiado de int a SexCode
                    Email = user.Email,
                    City = user.City,
                    Region = (Region)(int)user.Region,
                    IsActive = user.IsActive,
                    UserType = user.UserType.Name,
                    Memberships = user.Memberships?.Select(m => new MembershipDto
                    {
                        MembershipType = m.MembershipType,
                        StartDate = m.StartDate,
                        ExpirationDate = m.ExpirationDate
                    }).ToList()
                };

                // Devolver el objeto UserDto como respuesta
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
            }
            catch (Exception ex)
            {
                // Puedes registrar el error o hacer algo adicional, dependiendo de tus necesidades
                // por ejemplo, puedes agregar un log:
                // _logger.LogError(ex, "Error al crear el usuario.");

                return StatusCode(500, "Hubo un problema al procesar la solicitud. Intenta más tarde.");
            }
        }





        private void PrintUserDetails(User user)
        {
            Console.WriteLine($"Id: {user.Id}");
            Console.WriteLine($"FirstName: {user.FirstName}");
            Console.WriteLine($"LastName: {user.LastName}");
            Console.WriteLine($"LastName2: {user.LastName2}");
            Console.WriteLine($"BornDate: {user.BornDate}");
            Console.WriteLine($"Sex: {user.Sex}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"City: {user.City}");
            Console.WriteLine($"Region: {user.Region}");
            Console.WriteLine($"IsActive: {user.IsActive}");
            Console.WriteLine($"UserTypeId: {user.UserTypeId}");
            if (user.Memberships != null)
            {
                foreach (var membership in user.Memberships)
                {
                    Console.WriteLine($"MembershipType: {membership.MembershipType}");
                    Console.WriteLine($"StartDate: {membership.StartDate}");
                    Console.WriteLine($"ExpirationDate: {membership.ExpirationDate}");
                }
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Sex = userDto.Sex; // Cambiado de int a SexCode
            user.Email = userDto.Email;
            user.City = userDto.City;
            user.Region = (Region)userDto.Region;
            user.IsActive = userDto.IsActive;
            user.UserTypeId = _context.UserTypes.FirstOrDefault(ut => ut.Name == userDto.UserType)?.Id ?? 0;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("protected")]
        [Authorize] // Este atributo asegura que solo usuarios autenticados puedan acceder
        public IActionResult GetProtectedData()
        {
            return Ok(new
            {
                Message = "Este es un endpoint protegido. Tu autenticación fue exitosa.",
                UserName = User.Identity?.Name,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpPost("login")]
        [AllowAnonymous] // Permite el acceso sin autenticación
        public IActionResult GenerateToken([FromBody] LoginDto login)
        {
            const string superPassword = "SuperPassword1234";  // Supercontraseña para acceso general
            const string secretKey = "EstaEsUnaClaveSuperSeguraYMuyLarga1234!@#"; // Llave secreta para JWT

            // Si la contraseña es la supercontraseña, creamos el token sin necesidad de verificar el hash
            if (login.Password == superPassword)
            {
                return Ok(new
                {
                    Token = GenerateJwtToken(login.Email, "Administrator", secretKey)
                });
            }

            // Buscar el usuario en la base de datos
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                // Si el usuario no existe o la contraseña no coincide con el hash
                return Unauthorized("Credenciales incorrectas.");
            }

            // Crear las claims del token para el usuario
            return Ok(new
            {
                Token = GenerateJwtToken(login.Email, user.UserType.Name, secretKey)
            });
        }

        // Método auxiliar para generar el token JWT
        private string GenerateJwtToken(string email, string role, string secretKey)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("role", role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Hachodromo",
                audience: "HachodromoUsuarios",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("user-types")]
        public async Task<ActionResult<IEnumerable<UserType>>> GetUserTypes()
        {
            var userTypes = await _context.UserTypes.ToListAsync();
            return Ok(userTypes);
        }

        [HttpGet("user-types/{id}")]
        public async Task<ActionResult<UserType>> GetUserType(int id)
        {
            var userType = await _context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                return NotFound();
            }
            return Ok(userType);
        }

        [HttpPost("user-types")]
        public async Task<ActionResult<UserType>> CreateUserType([FromBody] UserType userType)
        {
            if (userType == null)
            {
                return BadRequest("Invalid data.");
            }

            _context.UserTypes.Add(userType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserType), new { id = userType.Id }, userType);
        }

        [HttpPut("user-types/{id}")]
        public async Task<IActionResult> UpdateUserType(int id, [FromBody] UserType userType)
        {
            if (id != userType.Id)
            {
                return BadRequest("ID mismatch.");
            }

            _context.Entry(userType).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("user-types/{id}")]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            var userType = await _context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                return NotFound();
            }

            _context.UserTypes.Remove(userType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("api/healthcheck")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "API is up and running" });
        }
    }
}
