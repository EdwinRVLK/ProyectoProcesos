using GimManager.Data;
using GimManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GimManager.Pages.Inventario
{
    public class AgregarProductoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public ProductoInput Producto { get; set; }

        // Constructor para inyectar ApplicationDbContext
        public AgregarProductoModel(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Crear una nueva instancia del producto
            var nuevoProducto = new Producto
            {
                Codigo = Producto.Codigo,
                NombreProducto = Producto.NombreProducto,
                Categoria = Producto.Categoria,
                Cantidad = Producto.Cantidad,
                Precio = Producto.Precio
            };

            // Agregar el producto a la base de datos
            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            // Mostrar un mensaje de éxito y redirigir al inventario
            TempData["Mensaje"] = "Producto agregado correctamente.";
            return RedirectToPage("/Inventario/Inventario");
        }
    }
}
