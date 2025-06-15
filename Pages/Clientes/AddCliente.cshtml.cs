using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GimManager.Pages.Clientes
{
    public class AgregarClienteModel : PageModel
    {
        [BindProperty]
        public ClienteInputModel Cliente { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Lógica para guardar el cliente
            TempData["Mensaje"] = "Cliente agregado exitosamente.";
            return RedirectToPage("/Clientes/AgregarCliente");
        }

        public class ClienteInputModel
        {
            [Required]
            public string Id { get; set; } = string.Empty;

            [Required]
            public string Nombre { get; set; } = string.Empty;

            [Required]
            public string Apellidos { get; set; } = string.Empty;

            [Required]
            public string Telefono { get; set; } = string.Empty;

            [Required]
            public string TipoMembresia { get; set; } = string.Empty;

            public IFormFile? Foto { get; set; }
        }
    }
}