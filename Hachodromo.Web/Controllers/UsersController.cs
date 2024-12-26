using Hachodromo.Shared.Constants;
using Hachodromo.Shared.DTOs;
using Hachodromo.Shared.Models;
using Hachodromo.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hachodromo.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
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
                    Email = user.Email,
                    City = user.City,
                    Region = (int)user.Region,
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
                    Email = user.Email,
                    City = user.City,
                    Region = (int)user.Region,
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

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                City = userDto.City,
                Region = (Region)userDto.Region,
                IsActive = userDto.IsActive,
                UserTypeId = _context.UserTypes.FirstOrDefault(ut => ut.Name == userDto.UserType)?.Id ?? 0,
                Memberships = userDto.Memberships?.Select(m => new Membership
                {
                    MembershipType = m.MembershipType,
                    StartDate = m.StartDate,
                    ExpirationDate = m.ExpirationDate
                }).ToList()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
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
    }
}
