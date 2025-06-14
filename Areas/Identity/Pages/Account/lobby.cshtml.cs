using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimManager.Pages.Inicio
{
    [Authorize]
    public class PrincipalModel : PageModel
    {
        public void OnGet()
        {
            // Aquí puedes agregar lógica para recuperar datos del usuario, rutinas, etc.
        }
    }
}
