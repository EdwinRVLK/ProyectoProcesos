using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimManager.Data;
using GimManager.Models;
using System.Text.Json;
using System.IO;

namespace GimManager.Pages.VentasYpagos
{
    [IgnoreAntiforgeryToken]
    public class PuntoDeVentaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PuntoDeVentaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnGetAgregarProducto(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return new JsonResult(new { success = false, message = "Código vacío." });

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (producto == null)
                return new JsonResult(new { success = false, message = "Producto no encontrado." });

            if (producto.Cantidad < 1)
                return new JsonResult(new { success = false, message = "Producto sin stock." });

            return new JsonResult(new
            {
                success = true,
                data = new
                {
                    codigo = producto.Codigo,
                    nombre = producto.NombreProducto,
                    precio = producto.Precio,
                    cantidad = 1
                }
            });
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostFinalizarVentaAsync()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                var detalles = JsonSerializer.Deserialize<List<DetalleVenta>>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (detalles == null || !detalles.Any())
                    return new JsonResult(new { success = false, message = "Carrito vacío." });

                using var tx = await _context.Database.BeginTransactionAsync();

                decimal total = detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    Total = total,
                    Detalles = detalles
                };

                foreach (var d in detalles)
                {
                    var producto = await _context.Productos.FirstAsync(p => p.Codigo == d.CodigoProducto);
                    producto.Cantidad -= d.Cantidad;
                    _context.Productos.Update(producto);
                }

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return new JsonResult(new { success = true, ventaId = venta.Id });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error del servidor: " + ex.Message });
            }
        }

        // Actualizamos este método para manejar "salidas" en vez de "entradas"
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostRegistrarSalidaAsync()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                var salida = JsonSerializer.Deserialize<Salida>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (salida == null || salida.Monto <= 0 || string.IsNullOrWhiteSpace(salida.Descripcion))
                    return new JsonResult(new { success = false, message = "Datos inválidos." });

                salida.Fecha = DateTime.Now;
                _context.Salidas.Add(salida);  // Guardamos la salida de dinero (gasto)
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostRegistrarEntradaAsync()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                var entrada = JsonSerializer.Deserialize<Entrada>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (entrada == null || entrada.Monto <= 0 || string.IsNullOrWhiteSpace(entrada.Descripcion))
                    return new JsonResult(new { success = false, message = "Datos inválidos." });

                entrada.Fecha = DateTime.Now;
                _context.Entradas.Add(entrada);  // Guardamos la entrada de dinero (ingreso)
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
