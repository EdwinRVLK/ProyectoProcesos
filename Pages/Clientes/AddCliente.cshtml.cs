using GimManager.Data;
using GimManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GimManager.Pages.Clientes
{
    public class AddClienteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public ClienteInputModel Cliente { get; set; } = new();

        // Propiedad para mostrar el total en la vista
        [BindProperty]
        public decimal TotalCalculado { get; set; }

        public AddClienteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verificar si el número de meses fue seleccionado
            if (Cliente.Meses == 0)
            {
                ModelState.AddModelError("Cliente.Meses", "Debe seleccionar el número de meses.");
                return Page();
            }

            // Crear un nuevo cliente
            var nuevoCliente = new Cliente
            {
                Nombre = Cliente.Nombre,
                Apellido = Cliente.Apellido,
                Telefono = Cliente.Telefono,
                TipoMembresia = Cliente.TipoMembresia
            };

            // Calcular el precio y la fecha de vencimiento
            nuevoCliente.CalcularPrecioYFechaVencimiento(Cliente.Meses);
            TotalCalculado = nuevoCliente.PrecioMembresia;

            // Guardar foto si se ha subido
            string rutaFoto = string.Empty;
            if (Cliente.Foto != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");
                var filePath = Path.Combine(uploadsFolder, Cliente.Foto.FileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Cliente.Foto.CopyToAsync(fileStream);
                }

                rutaFoto = "/imagenes/" + Cliente.Foto.FileName;
            }

            nuevoCliente.RutaFoto = rutaFoto;

            // 1. Guardar el cliente en la base de datos
            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            // 2. Registrar la venta de membresía
            var ventaMembresia = new VentaMembresia
            {
                Fecha = DateTime.Now,
                Total = TotalCalculado,
                TipoMembresia = nuevoCliente.TipoMembresia,
                Meses = Cliente.Meses,
                ClienteId = nuevoCliente.Id,
                Concepto = $"Membresía {nuevoCliente.TipoMembresia} ({Cliente.Meses} meses)"
            };

            _context.VentasMembresias.Add(ventaMembresia);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"Cliente agregado exitosamente. Venta registrada por ${TotalCalculado:0.00}";
            return RedirectToPage("/Clientes/GestionCliente");
        }

        // Modelo de entrada para el cliente
        public class ClienteInputModel
        {
            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string Nombre { get; set; } = string.Empty;

            [Required(ErrorMessage = "El apellido es obligatorio")]
            public string Apellido { get; set; } = string.Empty;

            [Required(ErrorMessage = "El teléfono es obligatorio")]
            [Phone(ErrorMessage = "Formato de teléfono inválido")]
            public string Telefono { get; set; } = string.Empty;

            [Required(ErrorMessage = "El tipo de membresía es obligatorio")]
            public string TipoMembresia { get; set; } = string.Empty;

            [Range(1, 12, ErrorMessage = "El número de meses debe estar entre 1 y 12")]
            public int Meses { get; set; }

            public IFormFile? Foto { get; set; }
        }
    }
}