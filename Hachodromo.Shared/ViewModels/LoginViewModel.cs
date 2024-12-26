   using System.ComponentModel.DataAnnotations;
   using System.Threading.Tasks;
   using Microsoft.AspNetCore.Components;

   namespace Hachodromo.Shared.ViewModels
   {
       public class LoginViewModel
       {
           [Required]
           [EmailAddress]
           public string Email { get; set; }

           [Required]
           public string Password { get; set; }

           public bool IsBusy { get; set; }

           public NavigationManager Navigation { get; set; }

           public async Task Login()
           {
               if (IsBusy)
                   return;

               IsBusy = true;

               // Implement your login logic here
               // For example, call an API to authenticate the user

               IsBusy = false;

               // Navigate to another page upon successful login
               Navigation.NavigateTo("/");
           }
       }
   }
   