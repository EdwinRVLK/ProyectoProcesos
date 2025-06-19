using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using Microsoft.EntityFrameworkCore;
using Internal;

namespace GimManager.Pages.Clientes
{
    public class RenovarClienteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RenovarClienteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        [BindProperty]
        public string TipoMembresia { get; set; } = string.Empty;

        [BindProperty]
        public int Meses { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Cliente = await _context.Clientes.FindAsync(id);
            if (Cliente == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Meses <= 0 || string.IsNullOrWhiteSpace(TipoMembresia))
            {
                Console.WriteLine("⚠️ Modelo inválido. Detalles:");

                foreach (var kvp in ModelState)
                {
                    var key = kvp.Key;
                    var errors = kvp.Value.Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($" - {key}: {error.ErrorMessage}");
                    }
                }
                ModelState.AddModelError(string.Empty, "Verifica los datos ingresados.");
                Cliente = await _context.Clientes.FindAsync(Cliente.Id);
                return Page();
            }

            var cliente = await _context.Clientes.FindAsync(Cliente.Id);
            if (cliente == null)
            {
                return NotFound();
            }

            decimal precio = TipoMembresia switch
            {
                "Normal" => 500 * Meses,
                "Estudiante" => 300 * Meses,
                "VIP" => 1000 * Meses,
                _ => throw new ArgumentException("Tipo de membresía inválido.")
            };

            // Actualizar cliente
            cliente.TipoMembresia = TipoMembresia;
            cliente.FechaVencimiento = DateTime.Now.AddMonths(Meses);
            cliente.PrecioMembresia = precio;

            // Registrar la venta
            var venta = new VentaMembresia
            {
                ClienteId = cliente.Id,
                Fecha = DateTime.Now,
                TipoMembresia = TipoMembresia,
                Meses = Meses,
                Total = precio,
                Concepto = $"Renovación de {Meses} mes(es)"
            };

            _context.VentasMembresias.Add(venta);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Membresía renovada con éxito.";
            return RedirectToPage("/Clientes/GestionCliente");
        }
    }
}
