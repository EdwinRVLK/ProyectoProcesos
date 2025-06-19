using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimManager.Data;
using GimManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace GimManager.Pages.Reportes
{
    public class GenerarReporteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GenerarReporteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Venta> Ventas { get; set; } = new();
        public List<VentaMembresia> VentasMembresia { get; set; } = new();

        public DateTime FechaHoy { get; set; } = DateTime.Today;

        public async Task OnGetAsync()
        {
            DateTime inicio = FechaHoy;
            DateTime fin = FechaHoy.AddDays(1);

            // Ventas de productos solo del día actual
            Ventas = await _context.Ventas
                .Where(v => v.Fecha >= inicio && v.Fecha < fin)
                .Include(v => v.Detalles)
                .ToListAsync();

            // Ventas de membresías solo del día actual
            VentasMembresia = await _context.VentasMembresias
                .Where(vm => vm.Fecha >= inicio && vm.Fecha < fin)
                .Include(vm => vm.Cliente)
                .ToListAsync();
        }
    }
}
