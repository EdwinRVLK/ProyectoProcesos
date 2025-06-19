using GimManager.Data;
using GimManager.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GimManager.Pages.Inicio
{
    public class PrincipalModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public int TotalUsuarios { get; set; }
        public int TotalEmpleados { get; set; }
        public decimal VentasDia { get; set; } // Total ventas del día
        public decimal VentasMes { get; set; } // Total ventas del mes

        public PrincipalModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            // Contar clientes
            TotalUsuarios = await _context.Clientes.CountAsync();
            // Contar empleados
            TotalEmpleados = await _context.Empleados.CountAsync();

            // Obtener el total de ventas realizadas hoy
            VentasDia = await _context.VentasMembresias
                .Where(v => v.Fecha.Date == System.DateTime.Now.Date)
                .SumAsync(v => v.Total);

            // Obtener el total de ventas realizadas este mes
            VentasMes = await _context.VentasMembresias
                .Where(v => v.Fecha.Month == System.DateTime.Now.Month && v.Fecha.Year == System.DateTime.Now.Year)
                .SumAsync(v => v.Total);
        }
    }
}
