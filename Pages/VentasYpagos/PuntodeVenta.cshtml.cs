using GimManager.Data;  // Para interactuar con la base de datos
using GimManager.Models;  // Usamos los modelos como Producto
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Necesario para los métodos asincrónicos de EF Core
using System.Collections.Generic;
using System.Linq;  // Necesario para LINQ (incluido FirstOrDefault)
using System.Threading.Tasks;

namespace GimManager.Pages.VentasYpagos
{
    public class PuntoDeVentaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        // Constructor para inyectar ApplicationDbContext
        public PuntoDeVentaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista de productos disponibles
        public List<Producto> Productos { get; set; } = new List<Producto>();

        // Lista para almacenar los productos agregados a la venta
        public List<Producto> Carrito { get; set; } = new List<Producto>();

        // Total acumulado de la venta
        public decimal TotalVenta { get; set; } = 0.00m;

        // Método OnGet para mostrar los productos disponibles
        public void OnGet()
        {
            // Recuperar todos los productos desde la base de datos
            Productos = _context.Productos.ToList();
        }

        // Método OnPost para agregar un producto al carrito de ventas
        public async Task<IActionResult> OnPostAgregarProductoAsync(string codigoProducto, int cantidad)
        {
            if (string.IsNullOrEmpty(codigoProducto) || cantidad <= 0)
            {
                TempData["Error"] = "El código del producto o la cantidad no son válidos.";
                return RedirectToPage();
            }

            // Buscar el producto en la base de datos por el código
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Codigo == codigoProducto);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToPage();
            }

            // Verificar que haya suficiente stock disponible
            if (producto.Cantidad < cantidad)
            {
                TempData["Error"] = "No hay suficiente stock disponible.";
                return RedirectToPage();
            }

            // Calcular el importe del producto
            decimal importeProducto = producto.Precio * cantidad;

            // Agregar el producto al carrito (simulando el proceso de venta)
            var productoCarrito = new Producto
            {
                Codigo = producto.Codigo,
                NombreProducto = producto.NombreProducto,
                Categoria = producto.Categoria,
                Cantidad = cantidad,
                Precio = producto.Precio,
                UltimaActualizacion = producto.UltimaActualizacion
            };

            Carrito.Add(productoCarrito); // Agregar al carrito de venta
            TotalVenta += importeProducto; // Acumulamos el total de la venta

            // Actualizamos la cantidad disponible en la base de datos
            producto.Cantidad -= cantidad;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync(); // Guardamos cambios en la base de datos

            TempData["Mensaje"] = "Producto agregado correctamente al carrito.";
            return RedirectToPage();
        }

        // Método para manejar el proceso de venta (puedes agregar detalles aquí)
        public async Task<IActionResult> OnPostCobrarAsync()
        {
            if (Carrito.Count == 0)
            {
                TempData["Error"] = "No hay productos en el carrito.";
                return RedirectToPage();
            }

            // Lógica para procesar el pago y finalizar la venta (esto puede incluir más detalles)
            // Puedes guardar los datos de la venta en la base de datos, por ejemplo en una tabla de Ventas.

            // Limpiar el carrito después de cobrar
            Carrito.Clear();
            TotalVenta = 0.00m; // Restablecer total de la venta

            TempData["Mensaje"] = "Venta realizada con éxito.";
            return RedirectToPage();
        }
    }
}
