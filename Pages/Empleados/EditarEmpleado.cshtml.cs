using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GimManager.Pages.Empleados
{
    public class EditarEmpleadoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Empleado Empleado { get; set; }

        public EditarEmpleadoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción GET para cargar los datos del producto por ID
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Buscar al producto en la base de datos por su ID
            Empleado = await _context.Empleados.FindAsync(id);

            // Si no se encuentra el producto, devolver NotFound
            if (Empleado == null)
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
            var empleadoExistente = await _context.Empleados.FindAsync(Empleado.Id);

            // Si el producto no existe, mostrar mensaje de error
            if (empleadoExistente == null)
            {
                TempData["ErrorMessage"] = "producto no encontrado.";
                return RedirectToPage("/Empleados/Empleados");
            }

            // Actualizar los campos editables del producto
            empleadoExistente.Nombre = Empleado.Nombre;
            empleadoExistente.ApellidoPaterno = Empleado.ApellidoPaterno;
            empleadoExistente.ApellidoMaterno = Empleado.ApellidoMaterno;
            empleadoExistente.Sueldo = Empleado.Sueldo;
            empleadoExistente.Rol = Empleado.Rol;
            empleadoExistente.FechaIngreso = Empleado.FechaIngreso;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Inventario actualizado correctamente";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventarioExists(Empleado.Id))
                {
                    TempData["ErrorMessage"] = "No se pudo actualizar el inventario. No existe.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al guardar los cambios.";
                }
            }
            return RedirectToPage("/Empleados/Empleados");

        }

        // Verificar si el producto existe en la base de datos
        private bool InventarioExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}