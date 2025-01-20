using Hachodromo.Shared.Constants;
using Hachodromo.Shared.DTOs;
using Hachodromo.Shared.Interfaces;
using Hachodromo.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hachodromo.Shared.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiRoute = "https://localhost:7125/";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var endpoint = "api/users/register";
                var response = await _httpClient.PostAsJsonAsync(_apiRoute + endpoint, createUserDto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al registrar el usuario. Código de estado: {response.StatusCode}, Contenido: {errorContent}");
                    return false;
                }

                return true;
            }
            catch (HttpRequestException httpEx)
            {
                // Manejar errores específicos de la solicitud HTTP, como problemas de red
                Console.WriteLine($"Error en la solicitud HTTP: {httpEx.Message}");
                return false; // O manejarlo de otra forma
            }
            catch (Exception ex)
            {
                // Manejar cualquier otra excepción no específica
                Console.WriteLine($"Error al registrar el usuario: {ex.Message}");
                return false; // O manejarlo de otra forma
            }
        }

        public async Task<List<UserType>?> GetUserTypesAsync()
        {
            try
            {
                var endpoint = "api/Users/user-types";
                    var userTypes = await _httpClient.GetFromJsonAsync<List<UserType>>(_apiRoute + endpoint);

                // Si la lista de tipos de usuario no es nula, excluimos el tipo "SuperAdmin"
                if (userTypes != null)
                {
                    userTypes = userTypes.Where(ut => ut.Name != "SuperAdmin").ToList();
                }

                return userTypes;
            }
            catch (HttpRequestException httpEx)
            {
                // Manejar errores específicos de la solicitud HTTP
                Console.WriteLine($"Error en la solicitud HTTP: {httpEx.Message}");
                return null; // O manejarlo de otra forma
            }
            catch (Exception ex)
            {
                // Manejar cualquier otra excepción
                Console.WriteLine($"Error al obtener los tipos de usuario: {ex.Message}");
                return null; // O manejarlo de otra forma
            }
        }
    }
}
