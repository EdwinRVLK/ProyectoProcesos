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
    public class ReportesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReportesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DateTime FechaDesde { get; set; } = DateTime.Today.AddMonths(-1);

        [BindProperty]
        public DateTime FechaHasta { get; set; } = DateTime.Today;

        public List<Reporte> Reportes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Reportes = await _context.Reportes
                .OrderByDescending(r => r.FechaGenerado)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FechaDesde > FechaHasta)
            {
                ModelState.AddModelError(string.Empty, "La fecha Desde no puede ser mayor que la Hasta.");
                await OnGetAsync();
                return Page();
            }

            DateTime inicio = FechaDesde.Date;
            DateTime fin = FechaHasta.Date.AddDays(1);

            // Obtener datos para el reporte
            var ventas = await _context.Ventas
                .Where(v => v.Fecha >= inicio && v.Fecha < fin)
                .Include(v => v.Detalles)
                .ToListAsync();

            var ventasMembresia = await _context.VentasMembresias
                .Where(vm => vm.Fecha >= inicio && vm.Fecha < fin)
                .ToListAsync();

            int totalProductos = ventas.Sum(v => v.Detalles.Sum(d => d.Cantidad));
            decimal ingresoProductos = ventas.Sum(v => v.Total);

            int totalMembresias = ventasMembresia.Count;
            decimal ingresoMembresias = ventasMembresia.Sum(vm => vm.Total);

            var nuevoReporte = new Reporte
            {
                FechaDesde = FechaDesde,
                FechaHasta = FechaHasta,
                TotalProductos = totalProductos,
                TotalMembresias = totalMembresias,
                IngresoProductos = ingresoProductos,
                IngresoMembresias = ingresoMembresias
            };

            _context.Reportes.Add(nuevoReporte);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
