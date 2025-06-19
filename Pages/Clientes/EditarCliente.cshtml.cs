using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GimManager.Pages.Clientes
{
    public class EditarClienteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Cliente Cliente { get; set; }

        public EditarClienteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción GET para cargar los datos del cliente por ID
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Buscar al cliente en la base de datos por su ID
            Cliente = await _context.Clientes.FindAsync(id);

            // Si no se encuentra el cliente, devolver NotFound
            if (Cliente == null)
            {
                return NotFound();
            }

            return Page();
        }

        // Acción POST para guardar los cambios en los datos del cliente
        public async Task<IActionResult> OnPostAsync()
        {
            // Verificar si el modelo es válido antes de guardar los cambios
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Buscar el cliente por ID
            var clienteExistente = await _context.Clientes.FindAsync(Cliente.Id);

            // Si el cliente no existe, mostrar mensaje de error
            if (clienteExistente == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToPage("/Clientes/GestionCliente");
            }

            // Actualizar los campos editables del cliente
            clienteExistente.Nombre = Cliente.Nombre;
            clienteExistente.Apellido = Cliente.Apellido;
            clienteExistente.Telefono = Cliente.Telefono;

            try
            {
                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Mostrar mensaje de éxito si los cambios se guardan
                TempData["SuccessMessage"] = "Cliente actualizado correctamente";
                return RedirectToPage("/Clientes/GestionCliente");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejo de errores si hay problemas de concurrencia
                if (!ClienteExists(Cliente.Id))
                {
                    TempData["ErrorMessage"] = "No se pudo actualizar el cliente. El cliente no existe.";
                    return RedirectToPage("/Clientes/GestionCliente");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al guardar los cambios.";
                    return RedirectToPage("/Clientes/GestionCliente");
                }
            }
        }

        // Verificar si el cliente existe en la base de datos
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
