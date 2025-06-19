using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GimManager.Pages.Inventario
{
    public class EditarProductoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Producto Producto { get; set; }

        public EditarProductoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción GET para cargar los datos del producto por ID
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Buscar al producto en la base de datos por su ID
            Producto = await _context.Productos.FindAsync(id);

            // Si no se encuentra el producto, devolver NotFound
            if (Producto == null)
            {
                return NotFound();
            }

            return Page();
        }

        // Acción POST para guardar los cambios en los datos del producto
        public async Task<IActionResult> OnPostAsync()
        {
            // Verificar si el modelo es válido antes de guardar los cambios
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Buscar el producto por ID
            var productoExistente = await _context.Productos.FindAsync(Producto.Id);

            // Si el producto no existe, mostrar mensaje de error
            if (productoExistente == null)
            {
                TempData["ErrorMessage"] = "producto no encontrado.";
                return RedirectToPage("/productos/Gestionproducto");
            }

            // Actualizar los campos editables del producto
            productoExistente.NombreProducto = Producto.NombreProducto;
            productoExistente.Categoria = Producto.Categoria;
            productoExistente.Cantidad = Producto.Cantidad;
            productoExistente.Precio = Producto.Precio;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Inventario actualizado correctamente";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventarioExists(Producto.Id))
                {
                    TempData["ErrorMessage"] = "No se pudo actualizar el inventario. No existe.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al guardar los cambios.";
                }
            }
            return RedirectToPage("/Inventario/Inventario");

        }

        // Verificar si el producto existe en la base de datos
        private bool InventarioExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}