using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimManager.Pages.Reportes
{
    public class ReportesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string TipoReporte { get; set; } = "Total de Ingresos";

        [BindProperty(SupportsGet = true)]
        public DateTime FechaDesde { get; set; } = DateTime.Now.AddMonths(-1);

        [BindProperty(SupportsGet = true)]
        public DateTime FechaHasta { get; set; } = DateTime.Now;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public List<Reporte> Reportes { get; set; }

        public void OnGet()
        {
            // Simulación de datos - reemplazar con acceso a base de datos real
            Reportes = ObtenerReportesFiltrados();
        }

        private List<Reporte> ObtenerReportesFiltrados()
        {
            // Aquí iría la lógica para filtrar según los parámetros
            // Esta es solo una simulación con datos de prueba
            return new List<Reporte>
            {
                new Reporte 
                { 
                    Fecha = DateTime.Now, 
                    MembresiasVendidas = 10, 
                    IngresosMembresias = 5000, 
                    ProductosVendidos = 20, 
                    IngresosProductos = 2000, 
                    IngresoTotal = 7000 
                },
                new Reporte 
                { 
                    Fecha = DateTime.Now.AddDays(-1), 
                    MembresiasVendidas = 5, 
                    IngresosMembresias = 2500, 
                    ProductosVendidos = 30, 
                    IngresosProductos = 3000, 
                    IngresoTotal = 5500 
                }
            };
        }
    }

    public class Reporte
    {
        public DateTime Fecha { get; set; }
        public int MembresiasVendidas { get; set; }
        public decimal IngresosMembresias { get; set; }
        public int ProductosVendidos { get; set; }
        public decimal IngresosProductos { get; set; }
        public decimal IngresoTotal { get; set; }
    }
}