using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GimManager.Pages.Reportes
{
    public class DetalleReporteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetalleReporteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Reporte Reporte { get; set; } = default!;

        public List<Venta> Ventas { get; set; } = new();
        public List<VentaMembresia> VentasMembresia { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Reporte = await _context.Reportes.FindAsync(Id);

            if (Reporte == null)
            {
                return NotFound();
            }

            DateTime inicio = Reporte.FechaDesde.Date;
            DateTime fin = Reporte.FechaHasta.Date.AddDays(1);

            Ventas = await _context.Ventas
                .Where(v => v.Fecha >= inicio && v.Fecha < fin)
                .Include(v => v.Detalles)
                .ToListAsync();

            VentasMembresia = await _context.VentasMembresias
                .Where(vm => vm.Fecha >= inicio && vm.Fecha < fin)
                .Include(vm => vm.Cliente)
                .ToListAsync();

            return Page();
        }
    }
}
