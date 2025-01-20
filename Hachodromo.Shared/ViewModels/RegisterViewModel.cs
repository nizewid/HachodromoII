// RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hachodromo.Shared.Models;
using Hachodromo.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;

public class RegisterViewModel
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;

    public RegisterViewModel(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
    }

    [Required]
    public User User { get; set; } = new User();

    public async Task HandleValidSubmit()
    {
        // Aqu� puedes llamar a tu servicio o backend para guardar el usuario
        // El password deber�a ser hasheado antes de guardarse en la base de datos.
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(User.PasswordHash);
        User.PasswordHash = hashedPassword;

        // Llama a tu API para guardar el usuario (ejemplo)
        var response = await _http.PostAsJsonAsync("api/users/register", User);
        if (response.IsSuccessStatusCode)
        {
            // Redirigir al usuario a la p�gina de inicio de sesi�n, o donde lo necesites
            _navigation.NavigateTo("/login");
        }
        else
        {
            // Maneja el error de registro (mostrar un mensaje, por ejemplo)
        }
    }
}
