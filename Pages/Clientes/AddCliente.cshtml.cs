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

            // Calcular el precio y la fecha de vencimiento usando el modelo
            nuevoCliente.CalcularPrecioYFechaVencimiento(Cliente.Meses);

            // Guardar foto si se ha subido
            string rutaFoto = string.Empty;
            if (Cliente.Foto != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");
                var filePath = Path.Combine(uploadsFolder, Cliente.Foto.FileName);

                // Asegurarse de que la carpeta exista
                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Cliente.Foto.CopyToAsync(fileStream);
                }

                rutaFoto = "/imagenes/" + Cliente.Foto.FileName; // Ruta relativa
            }

            // Asignar la ruta de la foto al cliente
            nuevoCliente.RutaFoto = rutaFoto;

            // Agregar el nuevo cliente a la base de datos
            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Cliente agregado exitosamente.";
            return RedirectToPage("/Clientes/AddCliente");
        }

        // Modelo de entrada para el cliente
        public class ClienteInputModel
        {
            [Required]
            public string Nombre { get; set; } = string.Empty;

            [Required]
            public string Apellido { get; set; } = string.Empty;

            [Required]
            public string Telefono { get; set; } = string.Empty;

            [Required]
            public string TipoMembresia { get; set; } = string.Empty;

            [Range(1, 12, ErrorMessage = "El número de meses debe estar entre 1 y 12.")]
            public int Meses { get; set; }  // Número de meses seleccionados por el usuario

            public IFormFile? Foto { get; set; }
        }
    }
}
