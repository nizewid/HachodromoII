using Hachodromo.Shared.DTOs;
using Hachodromo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hachodromo.Shared.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(CreateUserDto createUserDto);
        Task<List<UserType>?> GetUserTypesAsync();
    }

}
