using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimManager.Pages.Inventario
{
    public class AgregarProductoModel : PageModel
    {
        [BindProperty]
        public ProductoInput Producto { get; set; }

        public class ProductoInput
        {
            public string Codigo { get; set; }
            public string NombreProducto { get; set; }
            public string Categoria { get; set; }
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // Lógica para guardar producto (simulada)
            TempData["Mensaje"] = "Producto agregado correctamente.";

            return RedirectToPage("/Inventario/Inventario");
        }
    }
}
