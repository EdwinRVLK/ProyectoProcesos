using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimManager.Data;

namespace GimManager.Pages.VentasYpagos // 👈 corregido aquí
{
    [IgnoreAntiforgeryToken]
    public class InventarioAPIModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public InventarioAPIModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var productos = await _context.Productos
                .Select(p => new
                {
                    codigo = p.Codigo,
                    nombreProducto = p.NombreProducto,
                    cantidad = p.Cantidad
                }).ToListAsync();

            return new JsonResult(productos);
        }
    }
}
