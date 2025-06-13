using Microsoft.AspNetCore.Identity;

namespace GimManager.Models
{
    public class AppUser : IdentityUser
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
    }
}
