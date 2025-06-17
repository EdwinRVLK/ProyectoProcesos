using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimManager.Pages.Ventas
{
    public class MostrarPagosModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        public List<Pago> Pagos { get; set; } = new List<Pago>();

        public void OnGet()
        {
            // Datos de prueba para pagos
            var todosLosPagos = new List<Pago>
            {
                new Pago { Id = 1, Nombre = "Juan", ApellidoPaterno = "López", ApellidoMaterno = "Ruiz", Vencimiento = new DateTime(2026, 10, 02), PagoTotal = 1200.00M },
                new Pago { Id = 2, Nombre = "María", ApellidoPaterno = "Hernández", ApellidoMaterno = "López", Vencimiento = new DateTime(2026, 10, 03), PagoTotal = 3000.00M },
                new Pago { Id = 3, Nombre = "Carlos", ApellidoPaterno = "Díaz", ApellidoMaterno = "Casanova", Vencimiento = new DateTime(2026, 10, 04), PagoTotal = 400.00M },
                new Pago { Id = 4, Nombre = "José", ApellidoPaterno = "Díaz", ApellidoMaterno = "Estrada", Vencimiento = new DateTime(2026, 10, 05), PagoTotal = 2000.00M },
                new Pago { Id = 5, Nombre = "Fernando", ApellidoPaterno = "Sulvaran", ApellidoMaterno = "Maldonado", Vencimiento = new DateTime(2026, 10, 06), PagoTotal = 300.00M }
            };

            // Filtrar por monto si hay un filtro aplicado
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                decimal montoFiltro;
                bool isMontoValido = decimal.TryParse(SearchTerm, out montoFiltro);

                if (isMontoValido)
                {
                    Pagos = todosLosPagos.Where(p => p.PagoTotal <= montoFiltro).ToList();
                }
                else
                {
                    Pagos = todosLosPagos; // Si no es un número válido, mostrar todos los pagos
                }
            }
            else
            {
                Pagos = todosLosPagos; // Si no hay búsqueda, mostrar todos los pagos
            }
        }

        public class Pago
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public DateTime Vencimiento { get; set; }
            public decimal PagoTotal { get; set; }
        }
    }
}
